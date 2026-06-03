import { render, screen, waitFor } from '@testing-library/react'
import { describe, it, expect, vi, beforeEach } from 'vitest'
import NotaFiscalPage from '../NotaFiscalPage'

const nfsMock = [
  {
    numero: 1001, serieNotaFiscal: 'A', codigoFilial: 1, codigoFicha: 42,
    nomeFilial: 'Filial SP', nomeCliente: 'Cliente ABC', cnpjCliente: '12.345.678/0001-99',
    dataEmissao: '2026-06-01T00:00:00', cancelado: 0, impressoes: 1,
    valorTransmissao: 1500.0, ambiente: '1', modeloNf: '55',
    chaveNfe: null, protocolo: null,
  },
]

function mockFetchSuccess(nfs = nfsMock) {
  vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
    ok: true,
    json: () => Promise.resolve(nfs),
  }))
}

function mockFetchError() {
  vi.stubGlobal('fetch', vi.fn().mockResolvedValue({ ok: false, status: 500 }))
}

describe('NotaFiscalPage', () => {
  beforeEach(() => {
    vi.unstubAllGlobals()
  })

  it('exibe spinner enquanto os dados estão carregando', () => {
    vi.stubGlobal('fetch', vi.fn().mockReturnValue(new Promise(() => {})))
    render(<NotaFiscalPage />)
    expect(screen.getByText(/carregando dados/i)).toBeInTheDocument()
  })

  it('renderiza a lista de notas fiscais após carregamento', async () => {
    mockFetchSuccess()
    render(<NotaFiscalPage />)
    await waitFor(() => expect(screen.getByText('1001')).toBeInTheDocument())
    expect(screen.getByText('Cliente ABC')).toBeInTheDocument()
  })

  it('exibe badge ATIVA para NF não cancelada', async () => {
    mockFetchSuccess()
    render(<NotaFiscalPage />)
    await waitFor(() => screen.getByText('1001'))
    expect(screen.getByText('ATIVA')).toBeInTheDocument()
  })

  it('exibe erro quando a API retorna falha', async () => {
    mockFetchError()
    render(<NotaFiscalPage />)
    await waitFor(() =>
      expect(screen.getByText(/erro ao carregar dados/i)).toBeInTheDocument()
    )
  })
})
