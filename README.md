# Todo App

A simple Todo List application built with .NET Web API (backend) and React (frontend).

## 🚀 Quick Start

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (version 16 or higher)

### 1. Setup Backend (.NET API)

```bash
# Clone or create the project
git clone <your-repo-url>
cd TodoApp

# Navigate to the API folder
cd TodoApp.API

# Restore packages and run
dotnet restore
dotnet run
```

✅ **Backend will run on:** `https://localhost:5094`  
✅ **API Documentation:** `https://localhost:5094/swagger`

### 2. Setup Frontend (React)

Open a **new terminal** and run:

```bash
# Navigate to frontend folder
cd todo-frontend

# Install dependencies
npm install

# Start the React app
npm start
```

✅ **Frontend will run on:** `http://localhost:3000`

### 3. Use the App

1. Open your browser to `http://localhost:3000`
2. Start adding, editing, and managing your todos!

## 📁 Project Structure

```
TodoApp/
├── TodoApp.API/          # .NET Web API (Backend)
├── TodoApp.Core/         # Domain models
├── TodoApp.Application/  # Business logic
├── TodoApp.Infrastructure/ # Database & repositories
├── TodoApp.Tests/        # Unit tests
└── todo-frontend/        # React app (Frontend)
```

## 🔧 Development

### Run Backend Only
```bash
cd TodoApp.API
dotnet run
```

### Run Frontend Only
```bash
cd todo-frontend
npm start
```

### Run Tests
```bash
cd TodoApp.Tests
dotnet test
```

## 📊 Features

- ✅ Add new todos
- ✅ Edit existing todos
- ✅ Mark todos as complete
- ✅ Delete todos
- ✅ Filter todos (All/Active/Completed)
- ✅ Data persists in SQLite database

## 🛠️ Troubleshooting

### Backend Issues
- **Port already in use:** Change port in `launchSettings.json`
- **Database errors:** Delete `todos.db` file and restart

### Frontend Issues
- **Can't connect to API:** Make sure backend is running on `https://localhost:5094`
- **npm errors:** Try `npm install` again or delete `node_modules` folder

### CORS Errors
If you get CORS errors, make sure the API is configured to allow requests from `http://localhost:3000`

---

**That's it! 🎉**

Need help? Check the API documentation at `https://localhost:5094/swagger` when the backend is running.
