import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import React, { Suspense, lazy, useEffect } from "react"; 
import MainLayout from "./layouts/MainLayout";   
import { getUser, createUser } from "./services/api"; 
import "./styles/App.css";

const Answers = lazy(() => import("./components/Answers"));
const Questions = lazy(() => import("./components/Questions"));
const Quizzes = lazy(() => import("./components/Quizzes"));
const QuizGame = lazy(() => import("./components/QuizGame"));
const Home = lazy(() => import("./pages/Home"));
const News = lazy(() => import("./components/News"));
const AddNews = lazy(() => import("./components/AddNews"));

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
          const response = await getUser(userData.id);   
          if (response.status === 200) { 
            return;
          }
        } catch (error) {
          if (error.response?.status === 404) { 
              await createUser(userData);   
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
      <Suspense fallback={<div>Loading...</div>}>
        <MainLayout>
          { !isAuthenticated ? (
            <div>
              <button onClick={() => loginWithRedirect()}>Войти с Auth0</button>
            </div>
          ) : (
            <div>
              <h2>Hello, {user?.name}</h2>
              <p>Email: {user?.email}</p>
              <button onClick={() => logout({ returnTo: window.location.origin })}>
                Log out
              </button>
            </div>
          )}

          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/quizzes" element={isAuthenticated ? <Quizzes /> : <Home />} />
            <Route path="/quiz-game/:quizId" element={isAuthenticated ? <QuizGame /> : <Home />} />
            <Route path="/questions/:quizId" element={isAuthenticated ? <Questions /> : <Home />} />
            <Route path="/answers/:questionId" element={isAuthenticated ? <Answers /> : <Home />} />
            <Route path="/news" element={<News />} />
            <Route path="/AddNews" element={ <AddNews/>}  />
          </Routes>
        </MainLayout>
      </Suspense>
    </Router>
  );
}

export default App;
