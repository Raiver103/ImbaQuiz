// src/components/Profile.js
import React, { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import axios from 'axios';

const Profile = () => {
  const { user, isAuthenticated, getAccessTokenSilently, logout } = useAuth0();
  const [quizzes, setQuizzes] = useState([]);

  useEffect(() => {
    const fetchQuizzes = async () => {
      if (isAuthenticated) {
        const token = await getAccessTokenSilently();
        try {
          const response = await axios.get('https://localhost:7280/api/quizzes', {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
          setQuizzes(response.data);
        } catch (error) {
          console.error(error);
        }
      }
    };
    fetchQuizzes();
  }, [isAuthenticated, getAccessTokenSilently]);

  if (!isAuthenticated) {
    return (
      <div>
        <h2>Please log in first</h2>
        <button onClick={() => loginWithRedirect()}>Log in</button>
      </div>
    );
  }

  return (
    <div>
      <h2>Welcome, {user?.name}</h2>
      <p>Email: {user?.email}</p>
      <button onClick={() => logout({ returnTo: window.location.origin })}>
        Log out
      </button>

      <h3>Your Quizzes</h3>
      <ul>
        {quizzes.length > 0 ? (
          quizzes.map((quiz) => (
            <li key={quiz.id}>
              <h4>{quiz.title}</h4>
              <p>{quiz.description}</p>
            </li>
          ))
        ) : (
          <p>You don't have any quizzes yet.</p>
        )}
      </ul>
    </div>
  );
};

export default Profile;
