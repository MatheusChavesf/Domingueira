import { ref } from 'vue'
import { jogadoresApi } from '@/services/api'
import type { Jogador } from '@/types'
import axios from 'axios'

export function useJogadores() {
  const jogadores = ref<Jogador[]>([])
  const loading = ref(false)
  const erro = ref<string | null>(null)
  const errosValidacao = ref<Record<string, string[]>>({})

  async function carregar() {
    loading.value = true
    erro.value = null
    try {
      const response = await jogadoresApi.listar()
      jogadores.value = response.data
    } catch {
      erro.value = 'Não foi possível carregar a lista de jogadores'
    } finally {
      loading.value = false
    }
  }

  async function criar(nome: string): Promise<boolean> {
    errosValidacao.value = {}
    erro.value = null
    try {
      const response = await jogadoresApi.criar(nome)
      jogadores.value.push(response.data)
      return true
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 400) {
        const data = error.response.data
        if (data?.erros) {
          errosValidacao.value = data.erros
        } else if (data?.mensagem) {
          errosValidacao.value = { nome: [data.mensagem] }
        }
      } else {
        erro.value = 'Erro ao criar jogador'
      }
      return false
    }
  }

  async function excluir(id: string): Promise<boolean> {
    erro.value = null
    try {
      await jogadoresApi.excluir(id)
      jogadores.value = jogadores.value.filter(j => j.id !== id)
      return true
    } catch {
      erro.value = 'Erro ao excluir jogador'
      return false
    }
  }

  return {
    jogadores,
    loading,
    erro,
    errosValidacao,
    carregar,
    criar,
    excluir
  }
}
