<template>
    <div class="card" :style="{ backgroundColor: color }">
      <h3>{{ title }}</h3>
      <p>{{ value }}</p>
    </div>
  </template>
  
  <script>
  export default {
    props: { title: String, value: Number, color: String }
  };
  </script>
  
  <style scoped>
  .card {
    padding: 16px;
    border-radius: 10px;
    color: #fff;
    font-weight: bold;
    text-align: center;
    box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.2);
  }
  </style>
  