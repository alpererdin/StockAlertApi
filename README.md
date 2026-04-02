# Stock Alert API

![.NET Build](https://github.com/alpererdin/StockAlertApi/actions/workflows/dotnet.yml/badge.svg)

A real-time stock price monitoring API built with ASP.NET Core 8 and Clean Architecture.

This project focuses on backend reliability, alert business rules, and testable service design.
Users can create stock alerts, receive real-time SignalR notifications, and interact with JWT-protected endpoints.
A background service polls the Finnhub API periodically and triggers alerts automatically.

---

## Tech Stack

| Category | Technology |
|----------|-----------|
| Framework | ASP.NET Core 8 |
| Database | PostgreSQL 16 + EF Core 8 |
| Real-time | SignalR |
| Authentication | JWT Bearer |
| External API | Finnhub |
| Testing | xUnit, Moq, FluentAssertions |
| Deployment | Docker + Docker Compose |

---

## QA / Testing Focus

This project was also used as a backend testing playground for API and service-level validation.

Covered areas:
- Alert creation business rules
- Duplicate alert prevention
- Service-layer unit testing
- Edge-case validation for alert scenarios

Run tests:
dotnet test

---

## Architecture

Clean Architecture:

Core → Application → Infrastructure → API

---

## Quick Start

Option 1 — Docker

git clone https://github.com/alpererdin/StockAlertApi.git
cd StockAlertApi
echo "FINNHUB_API_KEY=your_api_key_here" > .env
docker-compose up -d

Swagger UI: http://localhost:5000/swagger

Option 2 — Manual

cd src/StockAlertApi.API
dotnet user-secrets set "Finnhub:ApiKey" "your_api_key_here"
docker run -d -p 5432:5432 -e POSTGRES_PASSWORD=postgres postgres:16
dotnet ef database update --project ../StockAlertApi.Infrastructure
dotnet run

---

## Sample Endpoints

POST /api/auth/register
POST /api/auth/login
POST /api/stocks
POST /api/alerts
GET /api/alerts

---

## Real-time Notifications

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/alertHub?userId={your-user-id}")
  .build();

connection.on("AlertTriggered", (data) => {
  console.log(`${data.stockSymbol} reached ${data.currentPrice}`);
});

connection.start();

Test page: http://localhost:5000/test.html
