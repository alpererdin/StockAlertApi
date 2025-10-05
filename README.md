[Deutsch](README.de.md)

#  Stock Alert API
A real-time stock price monitoring system with instant notifications built using Clean Architecture and modern .NET technologies.

###  Project Overview

This API allows users to:
- Set price alerts for stocks (e.g., "notify me when AAPL reaches $200")
- Receive real-time notifications via SignalR when price targets are hit
- Monitor stock prices through background service integration with Finnhub API
- Manage alerts with secure JWT authentication

---

###  Architecture
┌─────────────┐      ┌──────────────┐      ┌─────────────┐
│   Client    │◄────►│  SignalR Hub │◄────►│ Background  │
│  (Browser)  │      │              │      │  Service    │
└─────────────┘      └──────────────┘      └──────┬──────┘
       │                     │
┌──────▼──────┐              │
│ Controllers │              │
└──────┬──────┘              │
       │                     │
┌──────▼──────┐       ┌──────▼──────┐
│  Services   │◄──────┤ Finnhub API │
└──────┬──────┘       └─────────────┘
       │
┌──────▼──────┐
│  EF Core    │
└──────┬──────┘
       │
┌──────▼──────┐
│ PostgreSQL  │
└─────────────┘

**Clean Architecture Layers:**
- **Core**: Domain entities & interfaces (no dependencies)
- **Application**: Business logic & background services
- **Infrastructure**: EF Core, external APIs, security
- **API**: Controllers, SignalR hubs, DTOs

---

###  Key Features

- ✅ **JWT Authentication**: Secure user registration and login
- ✅ **Real-time Notifications**: SignalR WebSocket connections
- ✅ **Background Processing**: Automated price monitoring every minute
- ✅ **External API Integration**: Finnhub stock price data
- ✅ **Clean Architecture**: Testable and maintainable design
- ✅ **Docker Support**: Single-command deployment
- ✅ **Duplicate Prevention**: Prevents identical alerts

---

###  Tech Stack

| Category | Technology |
|----------|-----------|
| **Framework** | ASP.NET Core 8.0 |
| **Database** | PostgreSQL 16 |
| **ORM** | Entity Framework Core 8.0 |
| **Real-time** | SignalR |
| **Authentication** | JWT Bearer |
| **External API** | Finnhub Stock API |
| **Containerization** | Docker + Docker Compose |

---

###  Quick Start

#### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Finnhub API Key](https://finnhub.io/) (free tier)

#### Option 1: Docker (Recommended)
# 1. Clone repository
git clone https://github.com/[your-username]/StockAlertApi.git
cd StockAlertApi

# 2. Create .env file
echo "FINNHUB_API_KEY=your_api_key_here" > .env

# 3. Start with Docker
docker-compose up -d

# API: http://localhost:5000/swagger

####  Option 2: Manual Setup
# 1. Configure API Key (User Secrets)
cd src/StockAlertApi.API
dotnet user-secrets set "Finnhub:ApiKey" "your_api_key_here"

# 2. Start PostgreSQL
docker run -d -p 5432:5432 -e POSTGRES_PASSWORD=postgres postgres:16

# 3. Apply migrations
dotnet ef database update --project ../StockAlertApi.Infrastructure

# 4. Run API
dotnet run

 API Usage
1. Register
httpPOST /api/auth/register
Content-Type: application/json

{
  "username": "testuser",
  "email": "test@test.com",
  "password": "Test123!"
}
2. Login & Get Token
httpPOST /api/auth/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "Test123!"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "userId": "...",
  "username": "testuser"
}
3. Authorize in Swagger

Click "Authorize" button
Paste token (without "Bearer" prefix)

4. Create Stock
httpPOST /api/stocks
Authorization: Bearer {token}

{
  "tickerSymbol": "AAPL",
  "companyName": "Apple Inc."
}
5. Create Alert
httpPOST /api/alerts
Authorization: Bearer {token}

{
  "stockId": "...",
  "targetPrice": 200.50,
  "direction": 1
}
Direction: 1 = Above, 2 = Below

 Real-time Notifications
Connect to SignalR hub for live alerts:
javascriptconst connection = new signalR.HubConnectionBuilder()
    .withUrl("/alertHub?userId={your-user-id}")
    .build();

connection.on("AlertTriggered", (data) => {
    console.log(`Alert: ${data.stockSymbol} reached ${data.currentPrice}`);
});

connection.start();
Test page available at: http://localhost:5000/test.html


- **SignalR**: Server-to-client real-time communication
- **Background Services**: Long-running tasks with IHostedService
- **Clean Architecture**: Separation of concerns and dependency inversion
- **EF Core Migrations**: Version-controlled database schema
- **JWT Security**: Stateless authentication
- **Docker**: Containerization and orchestration

## Technical & Architectural Highlights

- **Clean Architecture:** The project is structured using Clean Architecture principles, separating concerns into `Core`, `Application`, `Infrastructure`, and `API` layers for high maintainability and testability.
- **Real-time Functionality:** Utilizes **SignalR** to push real-time price alert notifications from the server directly to connected clients.
- **Background Processing:** A hosted `BackgroundService` runs periodically to fetch external data and trigger alerts, demonstrating asynchronous, long-running task management.
- **Secure by Design:** Implements **JWT-based authentication** to protect endpoints and includes business logic to prevent data duplication.
- **Containerization:** The application is fully containerized using **Docker** and orchestrated with `docker-compose` for consistent and easy deployment.
- **External API Integration:** Demonstrates consumption of a third-party RESTful API (e.g., Finnhub) to fetch real-world financial data.
