<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useJogadores } from '@/composables/useJogadores'
import { useRegistros } from '@/composables/useRegistros'
import type { CriarRegistroDto } from '@/types'

const { jogadores, loading: loadingJogadores, erro: erroJogadores, carregar } = useJogadores()
const { loading, erro, errosValidacao, criar } = useRegistros()

const jogadorId = ref('')
const data = ref('')
const gols = ref<number>(0)
const assistencias = ref<number>(0)
const sucesso = ref(false)

onMounted(() => {
  carregar()
})

function formatarDataParaISO(dataInput: string): string {
  // Accepts DD/MM/AAAA and converts to YYYY-MM-DD
  const partes = dataInput.split('/')
  if (partes.length === 3) {
    const [dia, mes, ano] = partes
    return `${ano}-${mes.padStart(2, '0')}-${dia.padStart(2, '0')}`
  }
  // Already in ISO format or native date input format
  return dataInput
}

async function submeter() {
  sucesso.value = false

  const dataISO = formatarDataParaISO(data.value)

  const dto: CriarRegistroDto = {
    jogadorId: jogadorId.value,
    data: dataISO,
    gols: gols.value,
    assistencias: assistencias.value
  }

  const resultado = await criar(dto)

  if (resultado) {
    sucesso.value = true
    jogadorId.value = ''
    data.value = ''
    gols.value = 0
    assistencias.value = 0
  }
}
</script>

<template>
  <div class="p-4 max-w-lg mx-auto">
    <h1 class="text-2xl font-bold text-gray-800 mb-6">Registro de Partida</h1>

    <!-- Success message -->
    <div
      v-if="sucesso"
      class="mb-4 p-3 bg-green-100 border border-green-300 text-green-800 rounded"
      role="alert"
    >
      Registro salvo com sucesso!
    </div>

    <!-- General error -->
    <div
      v-if="erro"
      class="mb-4 p-3 bg-red-100 border border-red-300 text-red-800 rounded"
      role="alert"
    >
      {{ erro }}
    </div>

    <!-- Error loading players -->
    <div
      v-if="erroJogadores"
      class="mb-4 p-3 bg-red-100 border border-red-300 text-red-800 rounded"
      role="alert"
    >
      {{ erroJogadores }}
    </div>

    <form @submit.prevent="submeter" class="space-y-4">
      <!-- Player dropdown -->
      <div>
        <label for="jogador" class="block text-sm font-medium text-gray-700 mb-1">
          Jogador
        </label>
        <select
          id="jogador"
          v-model="jogadorId"
          class="w-full border border-gray-300 rounded px-3 py-2 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          :disabled="loadingJogadores"
        >
          <option value="" disabled>Selecione um jogador</option>
          <option v-for="jogador in jogadores" :key="jogador.id" :value="jogador.id">
            {{ jogador.nome }}
          </option>
        </select>
        <p
          v-if="errosValidacao['jogadorId']"
          class="mt-1 text-sm text-red-600"
          role="alert"
        >
          {{ errosValidacao['jogadorId'][0] }}
        </p>
      </div>

      <!-- Date field -->
      <div>
        <label for="data" class="block text-sm font-medium text-gray-700 mb-1">
          Data da Partida
        </label>
        <input
          id="data"
          v-model="data"
          type="text"
          placeholder="DD/MM/AAAA"
          class="w-full border border-gray-300 rounded px-3 py-2 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
        <p
          v-if="errosValidacao['data']"
          class="mt-1 text-sm text-red-600"
          role="alert"
        >
          {{ errosValidacao['data'][0] }}
        </p>
      </div>

      <!-- Gols input -->
      <div>
        <label for="gols" class="block text-sm font-medium text-gray-700 mb-1">
          Gols
        </label>
        <input
          id="gols"
          v-model.number="gols"
          type="number"
          min="0"
          max="99"
          class="w-full border border-gray-300 rounded px-3 py-2 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
        <p
          v-if="errosValidacao['gols']"
          class="mt-1 text-sm text-red-600"
          role="alert"
        >
          {{ errosValidacao['gols'][0] }}
        </p>
      </div>

      <!-- Assistencias input -->
      <div>
        <label for="assistencias" class="block text-sm font-medium text-gray-700 mb-1">
          Assistências
        </label>
        <input
          id="assistencias"
          v-model.number="assistencias"
          type="number"
          min="0"
          max="99"
          class="w-full border border-gray-300 rounded px-3 py-2 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        />
        <p
          v-if="errosValidacao['assistencias']"
          class="mt-1 text-sm text-red-600"
          role="alert"
        >
          {{ errosValidacao['assistencias'][0] }}
        </p>
      </div>

      <!-- Submit button -->
      <div>
        <button
          type="submit"
          :disabled="loading"
          class="w-full bg-blue-600 text-white font-medium py-2 px-4 rounded hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
        >
          <span v-if="loading">Registrando...</span>
          <span v-else>Registrar</span>
        </button>
      </div>
    </form>
  </div>
</template>
