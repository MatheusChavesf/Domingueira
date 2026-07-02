<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink, RouterView } from 'vue-router'

const mobileMenuOpen = ref(false)

function closeMobileMenu() {
  mobileMenuOpen.value = false
}
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Navigation -->
    <nav class="bg-green-700 text-white shadow-md">
      <div class="max-w-7xl mx-auto px-4">
        <div class="flex items-center justify-between h-14">
          <!-- App Title -->
          <div class="flex-shrink-0">
            <span class="text-lg font-bold tracking-tight">⚽ Domingueira</span>
          </div>

          <!-- Desktop Navigation Links -->
          <div class="hidden sm:flex sm:items-center sm:space-x-4">
            <RouterLink
              to="/"
              class="px-3 py-2 rounded-md text-sm font-medium hover:bg-green-600 transition-colors"
              active-class="bg-green-800"
              @click="closeMobileMenu"
            >
              Placar Geral
            </RouterLink>
            <RouterLink
              to="/jogadores"
              class="px-3 py-2 rounded-md text-sm font-medium hover:bg-green-600 transition-colors"
              active-class="bg-green-800"
              @click="closeMobileMenu"
            >
              Jogadores
            </RouterLink>
            <RouterLink
              to="/registros"
              class="px-3 py-2 rounded-md text-sm font-medium hover:bg-green-600 transition-colors"
              active-class="bg-green-800"
              @click="closeMobileMenu"
            >
              Registros
            </RouterLink>
          </div>

          <!-- Mobile Menu Button -->
          <div class="sm:hidden">
            <button
              type="button"
              class="inline-flex items-center justify-center p-2 rounded-md hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-white"
              aria-label="Abrir menu de navegação"
              :aria-expanded="mobileMenuOpen"
              @click="mobileMenuOpen = !mobileMenuOpen"
            >
              <!-- Hamburger icon -->
              <svg
                v-if="!mobileMenuOpen"
                class="h-6 w-6"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                aria-hidden="true"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
              </svg>
              <!-- Close icon -->
              <svg
                v-else
                class="h-6 w-6"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                aria-hidden="true"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
        </div>
      </div>

      <!-- Mobile Menu -->
      <div v-if="mobileMenuOpen" class="sm:hidden border-t border-green-600">
        <div class="px-2 pt-2 pb-3 space-y-1">
          <RouterLink
            to="/"
            class="block px-3 py-2 rounded-md text-base font-medium hover:bg-green-600 transition-colors"
            active-class="bg-green-800"
            @click="closeMobileMenu"
          >
            Placar Geral
          </RouterLink>
          <RouterLink
            to="/jogadores"
            class="block px-3 py-2 rounded-md text-base font-medium hover:bg-green-600 transition-colors"
            active-class="bg-green-800"
            @click="closeMobileMenu"
          >
            Jogadores
          </RouterLink>
          <RouterLink
            to="/registros"
            class="block px-3 py-2 rounded-md text-base font-medium hover:bg-green-600 transition-colors"
            active-class="bg-green-800"
            @click="closeMobileMenu"
          >
            Registros
          </RouterLink>
        </div>
      </div>
    </nav>

    <!-- Page Content -->
    <main class="max-w-7xl mx-auto">
      <RouterView />
    </main>
  </div>
</template>
