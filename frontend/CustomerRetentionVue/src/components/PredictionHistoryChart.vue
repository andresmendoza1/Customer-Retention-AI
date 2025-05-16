<template>
    <div class="chart-container">
      <canvas ref="historyCanvas"></canvas>
    </div>
  </template>
  
  <script>
  import { Chart, registerables } from 'chart.js';
  import { ref, watch, onMounted } from 'vue';
  
  export default {
    props: { historyData: Array },
    setup(props) {
      const historyCanvas = ref(null);
      let historyChartInstance = null;
  
      const renderChart = () => {
        if (historyChartInstance) {
          historyChartInstance.destroy();
        }
        Chart.register(...registerables);
        historyChartInstance = new Chart(historyCanvas.value, {
          type: 'line',
          data: {
            labels: props.historyData.map(p => new Date(p.createdAt).toLocaleDateString()),
            datasets: [{
              label: 'Current Customer Churn History Probability',
              data: props.historyData.map(p => p.predictionProbability),
              borderColor: '#ff4d4d',
              fill: false
            }]
          },
          options: {
            scales: {
              x: { title: { display: true, text: 'Date', font: { size: 14 } } },
              y: { title: { display: true, text: 'Probability (%)', font: { size: 14 } }, grid: { display: false } }
            }
          }
        });
      };
  
      onMounted(() => {
        renderChart();
      });
  
      watch(() => props.historyData, renderChart, { deep: true });
  
      return { historyCanvas };
    },
  };
  </script>
  
  