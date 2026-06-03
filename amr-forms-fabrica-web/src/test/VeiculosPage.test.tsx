import { render, screen, waitFor, fireEvent } from '@testing-library/react'
import { describe, it, expect, vi, beforeEach } from 'vitest'
import VeiculosPage from '../VeiculosPage'

// ── Dados mock ────────────────────────────────────────────────────────────────

const veiculosMock = [
  { placa: 'ABC-1234', codigoFilial: 1, ufVeiculo: 'SP', codigoRntc: 'RNT001' },
  { placa: 'DEF-5678', codigoFilial: 2, ufVeiculo: 'RJ', codigoRntc: null },
  { placa: 'GHI-9012', codigoFilial: 1, ufVeiculo: null, codigoRntc: null },
]

const filiaisMock = [
  { codigo: 1, nome: 'Filial São Paulo' },
  { codigo: 2, nome: 'Filial Campinas' },
]

// ── Helper: mock de fetch com sucesso ────────────────────────────────────────

function mockFetchSuccess(
  veiculos = veiculosMock,
  filiais = filiaisMock,
) {
  vi.stubGlobal(
    'fetch',
    vi.fn().mockImplementation((url: string) => {
      if (String(url).includes('/api/Veiculo')) {
        return Promise.resolve({ ok: true, json: () => Promise.resolve(veiculos) })
      }
      if (String(url).includes('/api/Filial')) {
        return Promise.resolve({ ok: true, json: () => Promise.resolve(filiais) })
      }
      return Promise.reject(new Error(`URL não mapeada: ${url}`))
    }),
  )
}

// ── Helper: mock de fetch com erro ───────────────────────────────────────────

function mockFetchError() {
  vi.stubGlobal(
    'fetch',
    vi.fn().mockResolvedValue({ ok: false, status: 500 }),
  )
}

// ─────────────────────────────────────────────────────────────────────────────

describe('VeiculosPage', () => {
  beforeEach(() => {
    vi.unstubAllGlobals()
  })

  it('exibe spinner enquanto os dados estão carregando', () => {
    vi.stubGlobal('fetch', vi.fn().mockReturnValue(new Promise(() => {})))
    render(<VeiculosPage />)
    expect(screen.getByText(/carregando dados/i)).toBeInTheDocument()
  })

  it('renderiza a lista de veículos após carregamento bem-sucedido', async () => {
    mockFetchSuccess()
    render(<VeiculosPage />)
    await waitFor(() => expect(screen.getByText('ABC-1234')).toBeInTheDocument())
    expect(screen.getByText('DEF-5678')).toBeInTheDocument()
    expect(screen.getByText('GHI-9012')).toBeInTheDocument()
  })

  it('exibe a contagem correta de veículos cadastrados', async () => {
    mockFetchSuccess()
    render(<VeiculosPage />)
    await waitFor(() =>
      expect(screen.getByText('3 veículos cadastrados')).toBeInTheDocument(),
    )
  })

  it('filtra veículos por busca de placa', async () => {
    mockFetchSuccess()
    render(<VeiculosPage />)
    await waitFor(() => screen.getByText('ABC-1234'))

    fireEvent.change(
      screen.getByPlaceholderText(/buscar por placa/i),
      { target: { value: 'ABC' } },
    )

    expect(screen.getByText('ABC-1234')).toBeInTheDocument()
    expect(screen.queryByText('DEF-5678')).not.toBeInTheDocument()
    expect(screen.queryByText('GHI-9012')).not.toBeInTheDocument()
  })

  it('exibe "Nenhum veículo encontrado" quando busca não tem resultados', async () => {
    mockFetchSuccess()
    render(<VeiculosPage />)
    await waitFor(() => screen.getByText('ABC-1234'))

    fireEvent.change(
      screen.getByPlaceholderText(/buscar por placa/i),
      { target: { value: 'ZZZNAOEXISTE' } },
    )

    expect(screen.getByText('Nenhum veículo encontrado.')).toBeInTheDocument()
  })

  it('exibe KPIs: Total, Com UF e Com RNTC com valores corretos', async () => {
    mockFetchSuccess()
    render(<VeiculosPage />)
    await waitFor(() => screen.getByText('ABC-1234'))

    // Total: 3 | Com UF: 2 (SP, RJ) | Com RNTC: 1 (RNT001)
    const kpiLabels = screen.getAllByText(/^(Total|Com UF|Com RNTC)$/i)
    expect(kpiLabels).toHaveLength(3)

    // Os valores numéricos dos KPIs
    const kpiValues = ['3', '2', '1']
    kpiValues.forEach(v => {
      expect(screen.getAllByText(v).length).toBeGreaterThanOrEqual(1)
    })
  })

  it('abre o modal de cadastro ao clicar em "+ Novo Veículo"', async () => {
    mockFetchSuccess()
    render(<VeiculosPage />)
    await waitFor(() => screen.getByText('ABC-1234'))

    fireEvent.click(screen.getByText('+ Novo Veículo'))

    expect(screen.getByText('Cadastrar Veículo')).toBeInTheDocument()
  })

  it('exibe caixa de erro quando a API retorna falha', async () => {
    mockFetchError()
    render(<VeiculosPage />)
    await waitFor(() =>
      expect(screen.getByText(/erro ao carregar dados/i)).toBeInTheDocument(),
    )
  })
})
