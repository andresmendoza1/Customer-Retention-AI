from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
import joblib
import pandas as pd
import json

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


model = joblib.load("models/best_model.joblib")

features = ["Tenure", "OrderCount", "DaySinceLastOrder", "CashbackAmount",
            "SatisfactionScore", "Complain", "HourSpendOnApp", "NumberOfDeviceRegistered"]

@app.post("/predict")
async def predict(data: dict):
    df = pd.DataFrame([data], columns=features)
    prediction = model.predict(df)[0]
    probability = model.predict_proba(df)[0][1]

    return {
        "prediction": int(prediction),
        "probability": float(probability)
    }

@app.get("/feature_importance")
async def get_feature_importance():
    with open("models/feature_importance.json", "r") as f:
        importance_data = json.load(f)
    return importance_data
