<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useJogadores } from '@/composables/useJogadores'

const { jogadores, loading, erro, errosValidacao, carregar, criar, excluir } = useJogadores()

const nomeInput = ref('')

async function handleSubmit() {
  const sucesso = await criar(nomeInput.value)
  if (sucesso) {
    nomeInput.value = ''
  }
}

async function handleExcluir(id: string) {
  await excluir(id)
}

onMounted(() => {
  carregar()
})
</script>

<template>
  <div class="p-4 sm:p-6 lg:p-8">
    <h1 class="text-2xl font-bold text-gray-800 mb-6">Gerenciar Jogadores</h1>

    <!-- Form to add player -->
    <form @submit.prevent="handleSubmit" class="mb-8">
      <div class="flex flex-col sm:flex-row gap-3">
        <div class="flex-1">
          <label for="nome-jogador" class="block text-sm font-medium text-gray-700 mb-1">
            Nome do Jogador
          </label>
          <input
            id="nome-jogador"
            v-model="nomeInput"
            type="text"
            placeholder="Digite o nome do jogador"
            class="w-full px-3 py-2 border rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-green-500 focus:border-green-500"
            :class="errosValidacao.nome ? 'border-red-500' : 'border-gray-300'"
          />
          <p
            v-if="errosValidacao.nome"
            class="mt-1 text-sm text-red-600"
            role="alert"
          >
            {{ errosValidacao.nome[0] }}
          </p>
        </div>
        <div class="sm:self-end">
          <button
            type="submit"
            class="w-full sm:w-auto px-4 py-2 bg-green-600 text-white font-medium rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-2 transition-colors"
          >
            Adicionar
          </button>
        </div>
      </div>
    </form>

    <!-- Loading state -->
    <div v-if="loading" class="text-center py-8">
      <p class="text-gray-500">Carregando jogadores...</p>
    </div>

    <!-- Error state -->
    <div v-else-if="erro" class="bg-red-50 border border-red-200 rounded-md p-4" role="alert">
      <p class="text-red-700">{{ erro }}</p>
    </div>

    <!-- Players list -->
    <div v-else>
      <div v-if="jogadores.length === 0" class="text-center py-8">
        <p class="text-gray-500">Nenhum jogador cadastrado.</p>
      </div>

      <ul v-else class="divide-y divide-gray-200 border border-gray-200 rounded-md">
        <li
          v-for="jogador in jogadores"
          :key="jogador.id"
          class="flex items-center justify-between px-4 py-3 hover:bg-gray-50"
        >
          <span class="text-gray-800 font-medium truncate mr-4">{{ jogador.nome }}</span>
          <button
            type="button"
            @click="handleExcluir(jogador.id)"
            class="flex-shrink-0 px-3 py-1 text-sm text-red-600 border border-red-300 rounded-md hover:bg-red-50 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 transition-colors"
            :aria-label="`Excluir jogador ${jogador.nome}`"
          >
            Excluir
          </button>
        </li>
      </ul>
    </div>
  </div>
</template>
