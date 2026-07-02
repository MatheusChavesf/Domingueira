import { ref } from 'vue'
import type { EstatisticaJogador } from '../types'
import { estatisticasApi } from '../services/api'

// Module-level (singleton) state — shared across all components that use this composable
const estatisticas = ref<EstatisticaJogador[]>([])
const loading = ref(false)
const erro = ref<string | null>(null)

export function useEstatisticas() {
  async function carregar() {
    loading.value = true
    erro.value = null
    try {
      const response = await estatisticasApi.obterPlacarGeral()
      estatisticas.value = response.data
    } catch (e: unknown) {
      erro.value = 'Não foi possível carregar o placar geral. Tente novamente.'
    } finally {
      loading.value = false
    }
  }

  return {
    estatisticas,
    loading,
    erro,
    carregar
  }
}
