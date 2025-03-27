import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react';
import './App.css';
import Quizzes from './components/Quizzes';
import Profile from './components/Profile';
import QuizCreate from './components/QuizCreate'; // Импортируем новый компонент
import QuestionCreate from './components/QuestionCreate'; // Импортируем новый компонент

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
                  {/* Используем Link для навигации */}
                  <li><Link to="/profile">Profile</Link></li>
                  <li><Link to="/quizzes">Create Quiz</Link></li>
                  <li><Link to="/create-quiz">New Quiz</Link></li> {/* Новый маршрут для создания квиза */}
                  <li><Link to="/create-question">New Question</Link></li> {/* Новый маршрут для создания вопроса */}
                </ul>
              </nav>
            </div>
          )}
        </header>

        <main>
          {/* Маршруты для компонентов */}
          <Routes>
            <Route path="/quizzes" element={<Quizzes />} />
            <Route path="/profile" element={<Profile />} />
            <Route path="/" element={<div>Welcome to the Imba Quiz App</div>} />
            <Route path="/create-quiz" element={<QuizCreate />} /> {/* Новый маршрут */}
            <Route path="/create-question" element={<QuestionCreate />} /> {/* Новый маршрут */}
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
