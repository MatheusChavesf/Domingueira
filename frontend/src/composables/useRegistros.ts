import { ref } from 'vue'
import { registrosApi } from '@/services/api'
import type { CriarRegistroDto, AtualizarRegistroDto } from '@/types'
import { useEstatisticas } from '@/composables/useEstatisticas'
import axios from 'axios'

export function useRegistros() {
  const loading = ref(false)
  const erro = ref<string | null>(null)
  const errosValidacao = ref<Record<string, string[]>>({})
  const { carregar: recarregarEstatisticas } = useEstatisticas()

  async function criar(dto: CriarRegistroDto): Promise<boolean> {
    loading.value = true
    erro.value = null
    errosValidacao.value = {}
    try {
      await registrosApi.criar(dto)
      await recarregarEstatisticas()
      return true
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        const status = error.response.status
        const data = error.response.data
        if (status === 400) {
          if (data?.erros) {
            errosValidacao.value = data.erros
          } else if (data?.mensagem) {
            erro.value = data.mensagem
          } else {
            erro.value = 'Dados inválidos. Verifique os campos e tente novamente.'
          }
        } else if (status === 404) {
          erro.value = data?.mensagem || 'Jogador não encontrado'
        } else {
          erro.value = 'Erro ao registrar partida'
        }
      } else {
        erro.value = 'Erro de conexão. Tente novamente.'
      }
      return false
    } finally {
      loading.value = false
    }
  }

  async function atualizar(id: string, dto: AtualizarRegistroDto): Promise<boolean> {
    loading.value = true
    erro.value = null
    errosValidacao.value = {}
    try {
      await registrosApi.atualizar(id, dto)
      await recarregarEstatisticas()
      return true
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        const status = error.response.status
        const data = error.response.data
        if (status === 400) {
          if (data?.erros) {
            errosValidacao.value = data.erros
          } else if (data?.mensagem) {
            erro.value = data.mensagem
          } else {
            erro.value = 'Dados inválidos. Verifique os campos e tente novamente.'
          }
        } else if (status === 404) {
          erro.value = data?.mensagem || 'Registro não encontrado'
        } else {
          erro.value = 'Erro ao atualizar registro'
        }
      } else {
        erro.value = 'Erro de conexão. Tente novamente.'
      }
      return false
    } finally {
      loading.value = false
    }
  }

  async function excluir(id: string): Promise<boolean> {
    loading.value = true
    erro.value = null
    errosValidacao.value = {}
    try {
      await registrosApi.excluir(id)
      await recarregarEstatisticas()
      return true
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        const status = error.response.status
        const data = error.response.data
        if (status === 404) {
          erro.value = data?.mensagem || 'Registro não encontrado'
        } else {
          erro.value = 'Erro ao excluir registro'
        }
      } else {
        erro.value = 'Erro de conexão. Tente novamente.'
      }
      return false
    } finally {
      loading.value = false
    }
  }

  return {
    loading,
    erro,
    errosValidacao,
    criar,
    atualizar,
    excluir
  }
}
