import axios from 'axios'
import type { Jogador, RegistroPartida, EstatisticaJogador, CriarRegistroDto, AtualizarRegistroDto } from '@/types'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
  headers: { 'Content-Type': 'application/json' }
})

export const jogadoresApi = {
  listar: () => api.get<Jogador[]>('/jogadores'),
  criar: (nome: string) => api.post<Jogador>('/jogadores', { nome }),
  excluir: (id: string) => api.delete(`/jogadores/${id}`)
}

export const registrosApi = {
  criar: (data: CriarRegistroDto) => api.post<RegistroPartida>('/registros', data),
  atualizar: (id: string, data: AtualizarRegistroDto) => api.put<RegistroPartida>(`/registros/${id}`, data),
  excluir: (id: string) => api.delete(`/registros/${id}`)
}

export const estatisticasApi = {
  obterPlacarGeral: () => api.get<EstatisticaJogador[]>('/estatisticas')
}

export default api
