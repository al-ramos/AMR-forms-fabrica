import axios from 'axios'

const erp = axios.create({
  baseURL: import.meta.env.VITE_ERP_URL || 'http://localhost:5186',
  headers: { 'Cache-Control': 'no-store' },
})

export interface PedidoVendaERP {
  id: number
  clienteId: number
  empresaId: number
  status: string
  total: number
  dataEmissao: string
  observacao?: string | null
  itens: {
    produtoId: number
    produtoNome: string
    quantidade: number
    precoUnitario: number
  }[]
}

export interface PedidoRds {
  codigo: number
  codigoFilial: number
  dataPedido: string | null
  codigoAddressNumber: number | null
  quantidadeTotalProdutos: number
  itens: {
    codigoPedido: number
    codigoProduto: number | null
    quantidade: number | null
    unidadeMedida: string | null
  }[]
}

// Adapta o contrato do SQLite para o contrato esperado pelo NovaFicha
function adaptarPedido(p: PedidoRds): PedidoVendaERP {
  return {
    id: p.codigo,
    clienteId: p.codigoAddressNumber ?? 0,
    empresaId: p.codigoFilial,
    status: 'Aprovado',
    total: p.itens.reduce((acc, i) => acc + (i.quantidade ?? 0), 0),
    dataEmissao: p.dataPedido ?? new Date().toISOString(),
    observacao: null,
    itens: p.itens.map(i => ({
      produtoId: i.codigoProduto ?? 0,
      produtoNome: `Produto #${i.codigoProduto ?? 0}`,
      quantidade: i.quantidade ?? 0,
      precoUnitario: 0,
    })),
  }
}

export const erpApi = {
  getPedidosAprovados: (cdFilial: number) =>
    erp.get<PedidoRds[]>(`/api/Pedido?cdFilial=${cdFilial}`)
       .then(r => ({ ...r, data: r.data.map(adaptarPedido) })),

  getPedido: (id: number) =>
    erp.get<PedidoRds>(`/api/Pedido/${id}`)
       .then(r => ({ ...r, data: adaptarPedido(r.data) })),
}

export default erp