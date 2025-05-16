import { createRouter, createWebHistory } from 'vue-router'
import ChurnPrediction from '../components/ChurnPrediction.vue'
import ChurnChart from '../components/ChurnChart.vue'
  
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: ChurnPrediction,
    },
    {
      path: '/chart',
      name: 'chart',
      component: ChurnChart,
    },
  ],
})

export default router
