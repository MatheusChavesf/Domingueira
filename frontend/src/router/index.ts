import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'placar-geral',
      component: () => import('../views/PlacarGeral.vue')
    },
    {
      path: '/jogadores',
      name: 'jogadores',
      component: () => import('../views/GerenciarJogadores.vue')
    },
    {
      path: '/registros',
      name: 'registros',
      component: () => import('../views/RegistroPartida.vue')
    }
  ]
})

export default router
