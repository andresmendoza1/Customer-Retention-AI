<template>
  <div>
    <h2>Predicción de abandono de clientes</h2>
    <input v-model="customerId" placeholder="Ingrese el Customer ID" />
    <button @click="fetchPrediction">Obtener Predicción</button>

    <div v-if="prediction">
      <p><strong>ID Cliente:</strong> {{ prediction.customerId }}</p>
      <p><strong>¿Abandono probable?</strong> {{ prediction.isChurnLikely ? 'Sí' : 'No' }}</p>
      <p><strong>Probabilidad de abandono:</strong> {{ (prediction.churnProbability * 100).toFixed(2) }}%</p>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue';
import { getChurnPrediction } from '../services/api';

export default {
  setup(_, { emit }) {
    const customerId = ref('');
    const prediction = ref(null);

    const fetchPrediction = async () => {
    try {
        prediction.value = await getChurnPrediction(customerId.value);
    } catch (error) {
        console.error('Error fetching prediction:', error);
        // Optionally, you can set prediction to null or show an error message in the UI
        prediction.value = null; // or handle it as needed
    }
    };

    return { customerId, prediction, fetchPrediction };
  }
};
</script>
