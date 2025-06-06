import { Link } from "react-router-dom";
import React from "react";

const MainLayout = ({ children }) => {
  return (
    <div className="app">
      <header className="app-header">
        <div className="header-content">
          <h1 className="logo">ImbaQuiz</h1>
          <nav className="main-nav">
            <Link to="/" className="nav-link">Main</Link>
            <Link to="/quizzes" className="nav-link">Quizzes</Link>
            <Link to="/news" className="nav-link">News</Link>
            <Link to="/AddNews" className="nav-link">AddNews</Link>
          </nav>
        </div>
      </header>

      <main className="content">
        <div className="content-container">
          {children}
        </div>
      </main>

      <footer className="app-footer">
        <p>© 2023 ImbaQuiz - Check your knowledge</p>
      </footer>
    </div>
  );
};

export default MainLayout;
