import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import React, { Suspense, lazy, useEffect } from "react"; 
import MainLayout from "./layouts/MainLayout";   
import { getUser, createUser } from "./services/api"; // Импортируем функции для работы с API
import "./styles/App.css";

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
          const response = await getUser(userData.id);  // Используем функцию для получения данных пользователя
          if (response.status === 200) { 
            return;
          }
        } catch (error) {
          if (error.response?.status === 404) { 
              await createUser(userData);  // Используем функцию для сохранения нового пользователя
          }
        }
      };
      saveUserToDb();
    }
  }, [user, isAuthenticated]);

  if (isLoading) {
    return <div>Загрузка...</div>;
  }

  return (
    <Router>
      <Suspense fallback={<div>Загрузка...</div>}>
        <MainLayout>
          { !isAuthenticated ? (
            <div>
              <button onClick={() => loginWithRedirect()}>Войти с Auth0</button>
            </div>
          ) : (
            <div>
              <h2>Привет, {user?.name}</h2>
              <p>Email: {user?.email}</p>
              <button onClick={() => logout({ returnTo: window.location.origin })}>
                Выйти
              </button>
            </div>
          )}

          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/quizzes" element={isAuthenticated ? <Quizzes /> : <Home />} />
            <Route path="/quiz-game/:quizId" element={isAuthenticated ? <QuizGame /> : <Home />} />
            <Route path="/questions/:quizId" element={isAuthenticated ? <Questions /> : <Home />} />
            <Route path="/answers/:questionId" element={isAuthenticated ? <Answers /> : <Home />} />
          </Routes>
        </MainLayout>
      </Suspense>
    </Router>
  );
}

export default App;
