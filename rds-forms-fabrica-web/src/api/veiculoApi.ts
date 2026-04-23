import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5186',
})

export interface Veiculo {
  cdPlacaVeiculo: string
  cdFilial: number
}

export const veiculoApi = {
  getByFilial: (cdFilial: number) =>
    api.get<Veiculo[]>(`/api/Veiculo/filial/${cdFilial}`),
  getAll: () =>
    api.get<Veiculo[]>('/api/Veiculo'),
}