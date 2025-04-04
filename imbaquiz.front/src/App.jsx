import { BrowserRouter as Router, Route, Routes, Link, useParams } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import "./styles/App.css";
import axios from "axios"; 
import React, { useEffect, Suspense, lazy } from "react";
 
const Answers = lazy(() => import("./components/Answers"));
const Questions = lazy(() => import("./components/Questions"));
const Quizzes = lazy(() => import("./components/Quizzes"));
const QuizGame = lazy(() => import("./components/QuizGame"));
const Home = lazy(() => import("./pages/Home"));

function App() {
  const { loginWithRedirect, logout, user, isAuthenticated, isLoading } = useAuth0();

  useEffect(() => {
    if (isAuthenticated && user) {
      const saveUserToDb = async () => {
        const userData = {
          id: user.sub,
          email: user.email || "No email provided",
          name: user.name || "No name provided",
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
 
  return (
    <Router>
      <header className="App-header">
        <h1>Добро пожаловать в Imba Quiz</h1>
        {!isAuthenticated ? (
          <button onClick={() => loginWithRedirect()}>Войти с Auth0</button>
        ) : (
          <div>
            <h2>Привет, {user?.name}</h2>
            <p>Email: {user?.email}</p>
            <button onClick={() => logout({ returnTo: window.location.origin })}>
              Выйти
            </button>
          </div>
        )}
      </header>

      <div className="app">
        <header className="app-header">
          <div className="header-content">
            <h1 className="logo">ImbaQuiz</h1>
            <nav className="main-nav">
              <Link to="/" className="nav-link">Главная</Link>
              <Link to="/quizzes" className="nav-link">Викторины</Link>
            </nav>
          </div>
        </header>

        <main className="content">
          <div className="content-container"> 
            <Suspense fallback={<div>Загрузка...</div>}>
              <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/quizzes" element={<Quizzes />} />
                <Route path="/quiz-game/:quizId" element={<QuizGame />} />  
                <Route path="/questions/:quizId" element={<QuestionWrapper />} /> 
                <Route path="/answers/:questionId" element={<AnswerWrapper />} /> 
              </Routes>
            </Suspense>
          </div>
        </main>

        <footer className="app-footer">
          <p>© 2023 ImbaQuiz - Проверь свои знания</p>
        </footer>
      </div>
    </Router>
  );
}
 
const QuestionWrapper = () => {
  const { quizId } = useParams();
  return <Questions quizId={quizId} />;
};

const AnswerWrapper = () => {
  const { questionId } = useParams();
  return <Answers questionId={questionId} />;
}; 

export default App;
