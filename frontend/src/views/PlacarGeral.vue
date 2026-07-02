<script setup lang="ts">
import { onMounted } from 'vue'
import { useEstatisticas } from '../composables/useEstatisticas'

const { estatisticas, loading, erro, carregar } = useEstatisticas()

onMounted(() => {
  carregar()
})
</script>

<template>
  <div class="p-4 max-w-4xl mx-auto">
    <h1 class="text-2xl font-bold text-gray-800 mb-6">Placar Geral</h1>

    <!-- Loading state -->
    <div v-if="loading" class="text-center py-8 text-gray-500">
      Carregando...
    </div>

    <!-- Error state -->
    <div v-else-if="erro" class="rounded-md bg-red-50 border border-red-200 p-4 text-red-700">
      {{ erro }}
    </div>

    <!-- Empty state -->
    <div v-else-if="estatisticas.length === 0" class="text-center py-8 text-gray-500">
      Nenhum jogador cadastrado
    </div>

    <!-- Leaderboard table -->
    <div v-else class="overflow-x-auto">
      <table class="w-full border-collapse text-left">
        <thead>
          <tr class="border-b-2 border-gray-200">
            <th class="px-4 py-3 text-sm font-semibold text-gray-600 w-12">#</th>
            <th class="px-4 py-3 text-sm font-semibold text-gray-600">Jogador</th>
            <th class="px-4 py-3 text-sm font-semibold text-gray-600 text-right">Gols</th>
            <th class="px-4 py-3 text-sm font-semibold text-gray-600 text-right">Assistências</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(jogador, index) in estatisticas"
            :key="jogador.nome"
            class="border-b border-gray-100 hover:bg-gray-50 transition-colors"
            :class="index % 2 === 0 ? 'bg-white' : 'bg-gray-50'"
          >
            <td class="px-4 py-3 text-sm text-gray-500 font-medium">{{ index + 1 }}</td>
            <td class="px-4 py-3 text-sm text-gray-800 font-medium">{{ jogador.nome }}</td>
            <td class="px-4 py-3 text-sm text-gray-700 text-right">{{ jogador.totalGols }}</td>
            <td class="px-4 py-3 text-sm text-gray-700 text-right">{{ jogador.totalAssistencias }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
