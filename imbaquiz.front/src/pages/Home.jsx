import React from "react";
import { Link } from "react-router-dom";

function Home() {
  return (
    <div className="home-hero">
      <h2>Welcome to Imba Quiz!</h2>
      <p className="hero-text">Create and manage quizzes with ease</p>
      <div className="cta-buttons">
        <Link to="/quizzes" className="cta-button primary">Explore Quizzes</Link>
      </div>
    </div>
  );
}

export default Home;
