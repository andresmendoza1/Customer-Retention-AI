<script>
import { ref } from 'vue';
import CustomerDataCard from './components/CustomerDataCard.vue';
import ChurnChart from './components/ChurnChart.vue';
import PredictionHistoryChart from './components/PredictionHistoryChart.vue';
import { getChurnPrediction, fetchPredictions, getPredictionHistory, savePrediction, getCustomerData, fetchFeatureImportance } from './services/api';

export default {
  components: { CustomerDataCard, ChurnChart, PredictionHistoryChart },

  setup() {
    const customerId = ref('');
    const predictionResult = ref({});
    const customerData = ref({});
    const storedPredictions = ref([]);
    const predictionHistory = ref([]);
    const predictionReady = ref(false); 
    const recoveryMessage = ref('');

    // 🔥 Al ingresar un Customer ID, se actualizan todos los datos del dashboard
    const handlePrediction = async () => {
      const prediction = await getChurnPrediction(customerId.value);
      const customerChurnData = await getCustomerData(customerId.value);

      predictionResult.value = prediction;
      customerData.value = customerChurnData;
      console.log(customerData.value);
      // 🔥 Vaciar y recargar los datos para evitar problemas de renderizado
      storedPredictions.value = [];
      predictionHistory.value = [];

      // 🔹 Guardar la predicción en CustomerChurnPrediction
      await savePrediction({
        customerid: customerId.value,
        prediction: prediction.isChurnLikely, 
        predictionprobability: (prediction.churnProbability * 100).toFixed(2)
      });

    // 🔹 Obtener el historial de predicciones
    predictionHistory.value = [...await getPredictionHistory(customerId.value)];

    // 🔹 Obtener clientes en riesgo vs estables
    storedPredictions.value = [...await fetchPredictions()];

    predictionReady.value = true; 

    console.log(predictionResult.value);
    if (!predictionResult.value.isChurnLikely) {
    recoveryMessage.value = "✅ No recovery action needed.";
  } else{
    // 🔥 Obtener importancia de características desde FastAPI
    const featureImportance = await fetchFeatureImportance();

    // 🔹 Ordenar características según su importancia
    const sortedFeatures = Object.entries(featureImportance)
      .sort((a, b) => b[1] - a[1]);
    
    console.log(sortedFeatures);
    // 🔥 Evaluar la estrategia más relevante según importancia
    for (const [feature, importance] of sortedFeatures) {
      if (feature === "Tenure" && customerData.value.tenure > 24) {
        recoveryMessage.value = "Offer a VIP membership for loyal customers!";
        break;
      } else if (feature === "CashbackAmount" && customerData.value.cashbackAmount > 50) {
        recoveryMessage.value = "Give an additional cashback bonus!";
        break;
      } else if (feature === "DaySinceLastOrder" && customerData.value.daysSinceLastOrder > 30) {
        recoveryMessage.value = "Send a personalized recommendation for their next order!";
        break;
      } else if (feature === "Complain" && customerData.value.complain) {
        recoveryMessage.value = "Contact customer support to resolve their issue!";
        break;
      } else if (feature === "SatisfactionScore" && customerData.value.satisfactionScore < 3) {
        recoveryMessage.value = "Offer a personalized discount to improve their experience!";
        break;
      } else if (feature === "OrderCount" && customerData.value.orderCount < 3) {
        recoveryMessage.value = "Exclusive offer: Discount on next order!";
        break;
      } else if (feature === "NumberOfDeviceRegistered" && customerData.value.numberOfDeviceRegistered < 2) {
        recoveryMessage.value = "Recommend using the app on multiple devices for a better experience!";
        break;
      } else if (feature === "HourSpendOnApp" && customerData.value.hourSpendOnApp < 5) {
        recoveryMessage.value = "Send relevant notifications to increase engagement!";
        break;
      }
    }

  }
     
    
    };

  return { customerId, customerData, storedPredictions, predictionHistory, handlePrediction, recoveryMessage, predictionReady, predictionResult };
  }
};
</script>

<template>
  <div class="container">

    <nav class="navbar">
      <h1>Customer Churn Dashboard</h1>
      <!-- 🔹 Input para ingresar el Customer ID -->
      <div class="search-bar">
        <input v-model="customerId" placeholder="Enter Customer ID" />
        <button @click="handlePrediction">Predict</button>
      </div>
    </nav>
    
    
    <div v-if="predictionReady" class="prediction-row">
      <div v-if="!predictionResult.isChurnLikely" class="prediction-card" style="background: #2ecc71;">
        ✅ Customer is not likely to churn
      </div>

      <div v-else class="prediction-row">
        <div class="prediction-card" style="background: #e74c3c;">
          ❌ Warning: Customer at risk of churn!
        </div>
        <div class="prediction-card" style="background: #f39c12;">
          📊 Churn Probability: {{ (predictionResult.churnProbability * 100).toFixed(2) }}%
        </div>
        <div class="prediction-card" style="background: #3498db;">
          🔥 Recovery Strategy: {{ recoveryMessage }}
        </div>
      </div>
    </div>


    <!-- 🔹 Cards con variables clave -->
    <div class="cards-grid">
      <CustomerDataCard title="Tenure" :value="customerData.tenure" color="#3498db" />
      <CustomerDataCard title="Devices Registered" :value="customerData.numberOfDeviceRegistered" color="#9b59b6" />
      <CustomerDataCard title="Satisfaction Score" :value="customerData.satisfactionScore" color="#2ecc71" />
      <CustomerDataCard title="Last Order" :value="customerData.daysSinceLastOrder" color="#f39c12" />
      <CustomerDataCard title="Cashback Amount" :value="customerData.cashbackAmount" color="#e74c3c" />
      <CustomerDataCard title="Complain" :value="customerData.complain ? 'Yes' : 'No'" color="#e67e22" />
      <CustomerDataCard title="Order Count" :value="customerData.orderCount" color="#1abc9c" />
      <CustomerDataCard title="Day Since Last Order" :value="customerData.daysSinceLastOrder" color="#8e44ad" />
    </div>
    
    <div class="charts-container">
      <!-- 🔹 Gráficos -->
      <ChurnChart :churnData="storedPredictions" />
      <PredictionHistoryChart :historyData="predictionHistory" />
    </div>

  </div>
</template>


<style scoped>

.charts-section {
  display: flex;
  justify-content: center;
  gap: 30px; /* 🔹 Espaciado mejorado */
  padding: 20px;
  background: #f8f9fa; /* 🔥 Fondo elegante */
  border-radius: 15px; /* 🔥 Bordes suaves */
  box-shadow: 2px 4px 10px rgba(0,0,0,0.2); /* 🔹 Efecto sombra */
}

  /* 🔹 Estilo del navbar */
  .container {
  width: 95vw; /* 🔹 Ocupa todo el ancho de la pantalla */
  height: 95vh; /* 🔹 Ocupa toda la altura */
  justify-content: center;
  align-items: center;
  padding: 20px;
  box-sizing: border-box;
}

.cards-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); /* 🔥 Asegura un ajuste flexible */
  gap: 20px;
  width: 100%; /* 🔹 Mantiene el ancho controlado */
}

.prediction-row {
  display: flex;
  justify-content: center;
  gap: 20px;
  width: 100%;
  margin-top: 20px;
}

.prediction-card {
  padding: 20px;
  border-radius: 10px;
  color: white;
  font-size: 1.2rem;
  text-align: center;
  font-weight: bold;
}
  .recovery-alert {
    position: fixed;
    top: 80px;
    left: 50%;
    transform: translateX(-50%);
    background: #f39c12;
    color: #fff;
    padding: 10px 20px;
    border-radius: 5px;
    box-shadow: 2px 2px 10px rgba(0,0,0,0.3);
    font-weight: bold;
  }

  .navbar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: #2c3e50;
    padding: 16px;
    border-radius: 10px;
    box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.2);
  }

  .navbar h1 {
    color: #ffffff;
    font-size: 24px;
    font-weight: bold;
  }

  .search-bar {
    display: flex;
    gap: 10px;
  }

  .search-bar input {
    padding: 8px;
    border-radius: 5px;
    border: none;
    width: 200px;
  }

  .search-bar button {
    background: #3498db;
    color: #fff;
    border: none;
    padding: 8px 16px;
    border-radius: 5px;
    cursor: pointer;
    font-weight: bold;
  }

  .search-bar button:hover {
    background: #2980b9;
  }

  /* 🔹 Grid de Cards */
  .cards-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 20px;
    margin: 30px 0;
  }

  /* 🔹 Contenedor de gráficos en una fila */
  .charts-container {
    display: flex;
    justify-content: space-between;
    gap: 20px;
  }

  .charts-container > * {
    width: 500px; /* 🔹 Tamaño estándar para ambas gráficas */
    height: 400px; /* 🔹 Mantener proporciones equilibradas */
 display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  }
</style>

