import pandas as pd
import numpy as np
import json 
import optuna
import joblib
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier 
from sklearn.metrics import classification_report, accuracy_score, recall_score

# Cargar datos de manera eficiente
features = ["Tenure", "OrderCount", "DaySinceLastOrder", "CashbackAmount",
            "SatisfactionScore", "Complain", "HourSpendOnApp", "NumberOfDeviceRegistered"]
df = pd.read_excel("datasets/customer_activity.xlsx", usecols=features + ["Churn"])
df.columns = df.columns.str.replace(' ', '')

# Preparar datos
data = df[features]
results = df["Churn"]

# División estratificada de datos
data_train, data_test, results_train, results_test = train_test_split(
    data, results, test_size=0.2, random_state=42, stratify=results
)

def objective(trial):
    params = {
        "n_estimators": trial.suggest_int("n_estimators", 50, 300),
        "max_depth": trial.suggest_int("max_depth", 3, 15),
        "min_samples_split": trial.suggest_int("min_samples_split", 2, 10),
        "min_samples_leaf": trial.suggest_int("min_samples_leaf", 1, 5),
        "class_weight": "balanced",
        "random_state": 42,
        "n_jobs": -1
    }
    
    model = RandomForestClassifier(**params)
    model.fit(data_train, results_train)
    results_pred = model.predict(data_test)
    return recall_score(results_test, results_pred)

# Optimización con Optuna
study = optuna.create_study(direction="maximize")
study.optimize(objective, n_trials=30)

# Entrenar modelo final
best_params = study.best_params
best_params.update({
    "class_weight": "balanced",
    "random_state": 42,
    "n_jobs": -1
})

best_model = RandomForestClassifier(**best_params)
best_model.fit(data_train, results_train)

# Evaluación
predictions = best_model.predict(data_test)
accuracy = accuracy_score(results_test, predictions)
recall = recall_score(results_test, predictions)

print("\nMejores hiperparámetros:")
for param, value in best_params.items():
    if param not in ['class_weight', 'random_state', 'n_jobs']:
        print(f"{param}: {value}")

print(f"\nMétricas finales:")
print(f"Accuracy: {accuracy:.3f}")
print(f"Recall: {recall:.3f}")
print("\nReporte de clasificación:")
print(classification_report(results_test, predictions))

# Análisis de importancia de características
feature_importance = pd.DataFrame({
    'feature': features,
    'importance': best_model.feature_importances_
})
print("\nImportancia de características:")
print(feature_importance.sort_values('importance', ascending=False))

# 🔹 Guardar en un JSON para FastAPI
feature_importance_dict = feature_importance.set_index('feature').to_dict()["importance"]

with open("models/feature_importance.json", "w") as f:
    json.dump(feature_importance_dict, f)

joblib.dump(best_model, "models/best_model.joblib")
print("Modelo guardado en models/best_model.joblib")
