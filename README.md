# 🌉 Bridge Monitor System (Digital Twin Prototype)

This project is a **Cyber-Physical System (CPS)** prototype designed to monitor the structural integrity of a bridge in real-time. It demonstrates a full-stack **IoT (Internet of Things)** architecture, utilizing a **Digital Twin** concept to visualize sensor data.

## 🚀 Project Architecture

The system follows a strict **N-Tier Architecture** to ensure scalability and separation of concerns:

`[IoT Simulator] ➡️ [SQL Server] ➡️ [ASP.NET Web API] ➡️ [Unity 3D Visualization]`

### 🛠️ Tech Stack
* **Data Producer:** C# Console Application (Simulates IoT Sensors with weighted randomness).
* **Persistence:** Microsoft SQL Server (MSSQL).
* **Middleware:** ASP.NET Core Web API (RESTful Service).
* **Digital Twin (Client):** Unity 3D Engine (Real-time data visualization via HTTP requests).

## ⚙️ How It Works

1.  **Simulation:** The Console Application generates synthetic stress data (MPa) for bridge pillars using a weighted probability algorithm (Simulating Normal, Warning, and Critical states).
2.  **Storage:** Data is securely stored in a SQL Server database with high-frequency timestamps.
3.  **API Layer:** An ASP.NET Core Web API exposes the latest sensor readings via REST endpoints (`GET /api/sensor/latest/{id}`), decoupling the database from the client.
4.  **Visualization:** Unity 3D polls the API asynchronously. The 3D bridge model changes color dynamically based on stress levels:
    * 🟢 **Green:** Safe (< 100 MPa)
    * 🟡 **Yellow:** Warning (100 - 120 MPa)
    * 🔴 **Red:** Critical / Danger (> 120 MPa)

## 📸 Features
* **Real-time Monitoring:** Updates visualization instantly as data flows.
* **Decoupled Architecture:** The Unity client has no direct dependency on the database (Security Best Practice).
* **Scalable Design:** New sensors can be added easily by extending the database and API models.

---
*This project was developed as a study on Software Architecture and IoT System Design.*
