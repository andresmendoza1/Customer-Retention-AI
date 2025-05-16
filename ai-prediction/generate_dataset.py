
#Using the Faker library to generate a dataset for customer activity, recovery messages, and engagement data.
import pandas as pd
from faker import Faker
import random


faker = Faker()
clients_number = 1000

# Generate fake customer data
customer_activity = {
    "id": [faker.uuid4() for _ in range(clients_number)],
    "visits_last_30_days": [random.randint(0, 30) for _ in range(clients_number)],
    "purchase_frequency": [random.randint(0, 10) for _ in range(clients_number)],
    "total_spent": [random.uniform(0, 1000) for _ in range(clients_number)],
    "time_since_last_purchase": [random.randint(0, 365) for _ in range(clients_number)],
    "Inactive":[random.choice([0, 1]) for _ in range(clients_number)],
}


dataFrame_activity = pd.DataFrame(customer_activity)
dataFrame_activity.to_csv("customer_activity.csv", index=False)

# Generate fake recovery messages data
recovery_messages = {
    "id": dataFrame_activity["id"],
    "inactive_probability": [round(random.uniform(0, 1), 2) for _ in range(clients_number)],
    "incentive_type": [random.choice(["discount", "recommendation", "email recovery"]) for _ in range(clients_number)],
    "response": [random.choice(["accepted", "declined"]) for _ in range(clients_number)],
}

dataFrame_recovery = pd.DataFrame(recovery_messages)
dataFrame_recovery.to_csv("recovery_messages.csv", index=False)

# Generate fake engagement data
engagement_data = {
    "id": dataFrame_activity["id"],
    "email_clicks": [random.randint(0, 10) for _ in range(clients_number)],
    "response_time": [random.randint(0, 48) for _ in range(clients_number)],
    "conversion": [random.choice([0, 1]) for _ in range(clients_number)],
}

dataFrame_engagement = pd.DataFrame(engagement_data)
dataFrame_engagement.to_csv("engagement_data.csv", index=False)

print("Datasets generated successfully!")

