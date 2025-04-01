import { BrowserRouter as Router, Route, Routes, Link, useParams } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import Answers from "./components/Answers";
import Questions from "./components/Questions";
import Quizzes from "./components/Quizzes";
import QuizGame from "./components/QuizGame"; // Подключаем игру
import "./App.css";
import React, { useEffect } from "react";
import axios from "axios"; 

function App() {
  const { loginWithRedirect, logout, user, isAuthenticated, isLoading } = useAuth0();

  useEffect(() => {
    if (isAuthenticated && user) {
      const saveUserToDb = async () => {
        const userData = {
          id: user.sub,
          email: user.email || "No email provided",
          name: user.name,
        };

        try {
          const response = await axios.get(`https://localhost:7280/api/users/${userData.id}`);
          if (response.status === 200) {
            console.log("Пользователь уже есть в БД.");
            return;
          }
        } catch (error) {
          if (error.response?.status === 404) {
            try {
              await axios.post("https://localhost:7280/api/users", userData);
              console.log("Пользователь сохранен в БД.");
            } catch (error) {
              console.error("Ошибка при сохранении пользователя:", error);
            }
          }
        }
      };
      saveUserToDb();
    }
  }, [user, isAuthenticated]);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <Router>
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
          </div>
        )}
      </header>

      <div className="app">
        <header className="app-header">
          <div className="header-content">
            <h1 className="logo">ImbaQuiz</h1>
            <nav className="main-nav">
              <Link to="/" className="nav-link">Home</Link>
              <Link to="/quizzes" className="nav-link">Quizzes</Link>
            </nav>
          </div>
        </header>

        <main className="content">
          <div className="content-container">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/quizzes" element={<Quizzes />} />
              <Route path="/quiz-game/:quizId" element={<QuizGame />} /> {/* Страница игры */}
              <Route path="/questions/:quizId" element={<QuestionWrapper />} /> {/* Страница вопросов */}
              <Route path="/answers/:questionId" element={<AnswerWrapper />} /> {/* Страница ответов */} 
            </Routes>
          </div>
        </main>

        <footer className="app-footer">
          <p>© 2023 ImbaQuiz - Test your knowledge</p>
        </footer>
      </div>
    </Router>
  );
}

// Обёртки для передачи параметров в компоненты
const QuestionWrapper = () => {
  const { quizId } = useParams();
  return <Questions quizId={quizId} />;
};

const AnswerWrapper = () => {
  const { questionId } = useParams();
  return <Answers questionId={questionId} />;
};

function Home() {
  return (
    <div className="home-hero">
      <h2>Welcome to Imba Quiz!</h2>
      <p className="hero-text">Create and manage quizzes with ease</p>
      <div className="cta-buttons">
        <Link to="/quizzes" className="cta-button primary">Explore Quizzes</Link>
        <Link to="/quiz-game/1" className="cta-button secondary">Start Quiz Game</Link> {/* Пример квиза */}
      </div>
    </div>
  );
}

export default App;
