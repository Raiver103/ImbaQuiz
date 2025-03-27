// src/App.js
import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom'; // Используем Link для маршрутизации
import { useAuth0 } from '@auth0/auth0-react';
import './App.css';
import Quizzes from './components/Quizzes'; 

function App() {
  const { loginWithRedirect, logout, user, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <Router>
      <div className="App">
        <header className="App-header">
          <h1>Welcome to Imba Quiz</h1>
          {!isAuthenticated ? (
            <button onClick={() => loginWithRedirect()}>Login with Auth0</button>
          ) : (
            <div>
              <h2>Hello, {user?.name}</h2>
              <p>Email: {user?.email}</p>
              <button onClick={() => logout({ returnTo: window.location.origin })}>
                Logout
              </button>
              <nav>
                <ul> 
                  <li>
                    <Link to="/profile">Profile</Link>
                  </li>
                  <li>
                    <Link to="/quizzes">Create Quiz</Link>
                  </li>
                </ul>
              </nav>
            </div>
          )}
        </header>

        <main>
          {/* Роутинг для компонентов */}
          <Routes>
            <Route path="/quizzes" element={<Quizzes />} /> 
            <Route path="/" element={<div>Welcome to the Imba Quiz App</div>} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
