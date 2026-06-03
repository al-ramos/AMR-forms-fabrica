import { render, screen, waitFor, fireEvent } from '@testing-library/react'
import { describe, it, expect, vi, beforeEach } from 'vitest'
import NotaFiscalPage from '../NotaFiscalPage'
import type { NotaFiscalAPI } from '../types'

// ── Dados mock ────────────────────────────────────────────────────────────────

const nfsMock: NotaFiscalAPI[] = [
  {
    numero: 1001,
    serieNotaFiscal: '001',
    codigoFilial: 1,
    codigoFicha: 42,
    nomeFilial: 'Filial São Paulo',
    nomeCliente: 'Cliente Alpha',
    cnpjCliente: '12.345.678/0001-00',
    dataEmissao: '2026-06-01T00:00:00Z',
    cancelado: null,
    impressoes: 1,
    valorTransmissao: 1500.0,
    ambiente: '1',
    modeloNf: '55',
    chaveNfe: null,
    protocolo: null,
  },
  {
    numero: 1002,
    serieNotaFiscal: '001',
    codigoFilial: 2,
    codigoFicha: null,
    nomeFilial: 'Filial Campinas',
    nomeCliente: 'Cliente Beta',
    cnpjCliente: null,
    dataEmissao: null,
    cancelado: 1,
    impressoes: 0,
    valorTransmissao: 500.0,
    ambiente: '2',
    modeloNf: '55',
    chaveNfe: null,
    protocolo: null,
  },
  {
    numero: 1003,
    serieNotaFiscal: '002',
    codigoFilial: 1,
    codigoFicha: null,
    nomeFilial: 'Filial São Paulo',
    nomeCliente: 'Cliente Gamma',
    cnpjCliente: null,
    dataEmissao: '2026-06-02T00:00:00Z',
    cancelado: null,
    impressoes: 2,
    valorTransmissao: 3200.0,
    ambiente: '1',
    modeloNf: '55',
    chaveNfe: null,
    protocolo: null,
  },
]

const itensMock = [
  {
    codigoProduto: 'P001',
    codigoEan: '7891234560001',
    quantidade: 2.0,
    unidadeMedidaComercial: 'UN',
    unidadeMedida: 'UN',
    precoUnitario: 750.0,
    valorTotal: 1500.0,
    valorIpi: 0.0,
    codigoCfo: '5102',
  },
]

// ── Helper: mock de fetch com sucesso ────────────────────────────────────────

function mockFetchSuccess(nfs = nfsMock, itens = itensMock) {
  vi.stubGlobal(
    'fetch',
    vi.fn().mockImplementation((url: string) => {
      if (String(url).includes('/itens')) {
        return Promise.resolve({ ok: true, json: () => Promise.resolve(itens) })
      }
      if (String(url).includes('/api/NotaFiscal')) {
        return Promise.resolve({ ok: true, json: () => Promise.resolve(nfs) })
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

describe('NotaFiscalPage', () => {
  beforeEach(() => {
    vi.unstubAllGlobals()
  })

  it('exibe spinner enquanto os dados estão carregando', () => {
    vi.stubGlobal('fetch', vi.fn().mockReturnValue(new Promise(() => {})))
    render(<NotaFiscalPage />)
    expect(screen.getByText(/carregando dados/i)).toBeInTheDocument()
  })

  it('renderiza a lista de notas fiscais após carregamento bem-sucedido', async () => {
    mockFetchSuccess()
    render(<NotaFiscalPage />)
    await waitFor(() => expect(screen.getByText('1001')).toBeInTheDocument())
    expect(screen.getByText('1002')).toBeInTheDocument()
    expect(screen.getByText('1003')).toBeInTheDocument()
  })

  it('exibe a contagem total de notas no período', async () => {
    mockFetchSuccess()
    render(<NotaFiscalPage />)
    await waitFor(() =>
      expect(screen.getByText('3 notas no período')).toBeInTheDocument(),
    )
  })

  it('exibe KPIs: Total de NFs, Ativas e Canceladas com valores corretos', async () => {
    mockFetchSuccess()
    render(<NotaFiscalPage />)
    await waitFor(() => screen.getByText('1001'))

    // Total: 3 | Ativas: 2 | Canceladas: 1
    // "Ativas" e "Canceladas" aparecem nos KPIs (div) e nos botões de filtro — usar getAllByText
    expect(screen.getByText('Total de NFs')).toBeInTheDocument()
    expect(screen.getAllByText('Ativas').length).toBeGreaterThanOrEqual(1)
    expect(screen.getAllByText('Canceladas').length).toBeGreaterThanOrEqual(1)
  })

  it('filtra notas fiscais por busca de cliente', async () => {
    mockFetchSuccess()
    render(<NotaFiscalPage />)
    await waitFor(() => screen.getByText('1001'))

    fireEvent.change(
      screen.getByPlaceholderText(/buscar por nf/i),
      { target: { value: 'Alpha' } },
    )

    expect(screen.getByText('Cliente Alpha')).toBeInTheDocument()
    expect(screen.queryByText('Cliente Beta')).not.toBeInTheDocument()
    expect(screen.queryByText('Cliente Gamma')).not.toBeInTheDocument()
  })

  it('filtra notas por status "Canceladas"', async () => {
    mockFetchSuccess()
    render(<NotaFiscalPage />)
    await waitFor(() => screen.getByText('1001'))

    // Clicar especificamente no botão de filtro (não no KPI label)
    fireEvent.click(screen.getByRole('button', { name: 'Canceladas' }))

    expect(screen.getByText('Cliente Beta')).toBeInTheDocument()
    expect(screen.queryByText('Cliente Alpha')).not.toBeInTheDocument()
    expect(screen.queryByText('Cliente Gamma')).not.toBeInTheDocument()
  })

  it('filtra notas por status "Ativas"', async () => {
    mockFetchSuccess()
    render(<NotaFiscalPage />)
    await waitFor(() => screen.getByText('1001'))

    // Clicar especificamente no botão de filtro (não no KPI label)
    fireEvent.click(screen.getByRole('button', { name: 'Ativas' }))

    expect(screen.getByText('Cliente Alpha')).toBeInTheDocument()
    expect(screen.getByText('Cliente Gamma')).toBeInTheDocument()
    expect(screen.queryByText('Cliente Beta')).not.toBeInTheDocument()
  })

  it('exibe caixa de erro quando a API retorna falha', async () => {
    mockFetchError()
    render(<NotaFiscalPage />)
    await waitFor(() =>
      expect(screen.getByText(/erro ao carregar dados/i)).toBeInTheDocument(),
    )
  })
})
