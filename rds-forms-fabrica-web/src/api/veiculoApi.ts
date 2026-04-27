import api from './api'

export interface Veiculo {
  cdPlacaVeiculo: string
  cdFilial: number
}

export const veiculoApi = {
  getByFilial: (cdFilial: number) => api.get<Veiculo[]>(`/api/Veiculo/filial/${cdFilial}`),
  getAll: () => api.get<Veiculo[]>('/api/Veiculo'),
}
