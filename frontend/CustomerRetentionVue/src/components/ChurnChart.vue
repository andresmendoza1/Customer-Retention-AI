<template>
    <div class="chart-container">
      <canvas ref="chartCanvas"></canvas>
    </div>
  </template>
  
  <script>
  import { Chart, registerables } from 'chart.js';
  import { ref, watch, onMounted } from 'vue';
  
  export default {
    props: { churnData: Array },
    setup(props) {
      const chartCanvas = ref(null);
      let churnChartInstance = null;
  
      const renderChart = () => {
        if (churnChartInstance) {
          churnChartInstance.destroy();
        }
        Chart.register(...registerables);
        churnChartInstance = new Chart(chartCanvas.value, {
          type: 'doughnut',
          data: {
            labels: ['Total Clients At Risk', 'Total Stable Clients'],
            datasets: [{
              data: [
                props.churnData.filter(c => c.prediction).length,
                props.churnData.filter(c => !c.prediction).length
              ],
              backgroundColor: ['#e74c3c', '#2ecc71'],
              hoverOffset: 4
            }],
          },
          options: {
            plugins: {
              legend: { labels: { font: { size: 14 } } }
            },
            responsive: false, // 🔥 Desactiva ajuste automático
            maintainAspectRatio: false, // 🔥 Permite tamaño personalizado
            width: 300,
            height: 300
          }
        });
      };
  
      onMounted(() => {
        renderChart();
      });
  
      watch(() => props.churnData, renderChart, { deep: true });
  
      return { chartCanvas };
    },
  };
  </script>
  