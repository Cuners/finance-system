# Finance Management System

A comprehensive finance management application built with a React frontend and .NET backend services.

## Project Structure

### Client (React/Vite)
- **Frontend Framework**: React with TypeScript
- **Styling**: CSS modules and component-based styling
- **State Management**: Custom hooks for data fetching and state management
- **API Communication**: Service layer for interacting with backend APIs

Key components:
- Authorization pages (login/register)
- Main dashboard with financial overview
- Transaction management
- Budget tracking
- Category management
- Wallet functionality

### Backend Services

#### AuthService
- User authentication and authorization
- JWT token management
- User registration and login functionality
- Role-based access control

#### FinanceService
- Financial data management
- Transaction processing
- Budget tracking
- Account management
- Category classification

### Architecture
- Clean Architecture principles
- Domain-driven design
- Repository pattern
- Unit of work pattern
- Caching mechanisms

## Technologies Used

### Frontend
- React 18+
- TypeScript
- Vite build tool
- CSS Modules
- Custom SVG icons

### Backend
- .NET 10
- Entity Framework Core
- SQL Server (likely)
- JWT Authentication
- Docker containerization

### Infrastructure
- Docker & docker-compose
- Nginx (reverse proxy/load balancer)

## Setup Instructions

### Prerequisites
- Node.js 18+
- .NET SDK 10+
- Docker & Docker Compose
- SQL Server (or compatible database)

### Development Setup

1. Clone the repository
```bash
git clone <repository-url>
cd finance-system
```

2. Set up the backend services
```bash
# Navigate to AuthService
cd AuthService
dotnet restore
dotnet build

# Navigate to FinanceService
cd ../FinanceService
dotnet restore
dotnet build
```

3. Set up the frontend
```bash
cd client
npm install
```

4. Configure environment variables
- Update connection strings in `appsettings.json` files
- Set JWT secret and other configuration values

5. Run the application
```bash
# Start backend services with Docker
docker-compose up --build

# In a separate terminal, start the frontend
cd client
npm run dev
```

## Features

- User authentication and authorization
- Dashboard with financial overview
- Transaction management (add, edit, delete)
- Budget planning and tracking
- Expense categorization
- Account management
- Real-time financial summaries

## API Endpoints

### Auth Service
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `POST /api/auth/logout` - User logout

### Finance Service
- `GET /api/transactions` - Get transactions
- `POST /api/transactions` - Create transaction
- `PUT /api/transactions/{id}` - Update transaction
- `DELETE /api/transactions/{id}` - Delete transaction
- `GET /api/budgets` - Get budgets
- `POST /api/budgets` - Create budget
- `PUT /api/budgets/{id}` - Update budget
- `DELETE /api/budgets/{id}` - Delete budget
- `GET /api/categories` - Get categories
- `GET /api/accounts` - Get accounts

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Commit your changes (`git commit -m 'Add amazing feature'`)
5. Push to the branch (`git push origin feature/amazing-feature`)
6. Open a Pull Request

## License

This project is licensed under the MIT License.