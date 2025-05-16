import axios from "axios";

const apiUrl = axios.create({
    baseURL: "http://localhost:5000",
})

const fastApiUrl = axios.create({
    baseURL: "http://127.0.0.1:8000/",
})

export const getChurnPrediction = async (customerId) =>{
    try{
        const response = await apiUrl.post('api/customers/churn-prediction', customerId, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
        return response.data;
    }catch(error){
        console.error('Error fetching churn prediction:', error);
        throw error;
    }
}

export const getCustomerData = async (customerId) => {
    try {
        const response = await apiUrl.get(`api/customers/customer-data/${customerId}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching customer data:', error);
        throw error;
    }
}

export const savePrediction = async (predictionData) => {
    await apiUrl.post(`api/customers/save-prediction`, predictionData);
  };
  
  export const getPredictionHistory = async (customerId) => {
    const response = await apiUrl.get(`api/customers/all-predictions/${customerId}`);
    return response.data;
  };
  
  export const fetchPredictions = async () => {
    const response = await apiUrl.get(`api/customers/predictions`);
        return response.data;
    };

    export const fetchFeatureImportance = async () => {
        try {
          const response = await fastApiUrl.get(`feature_importance`);
          return response.data;
        } catch (error) {
          console.error("Error fetching feature importance:", error);
          return {};
        }
      };