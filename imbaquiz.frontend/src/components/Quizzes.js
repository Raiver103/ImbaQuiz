// src/components/Quizzes.js
import React, { useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import axios from 'axios';

const Quizzes = () => {
  const { getAccessTokenSilently } = useAuth0();
  const [quizTitle, setQuizTitle] = useState('');
  const [quizDescription, setQuizDescription] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();

    const token = await getAccessTokenSilently();
    const newQuiz = {
      title: quizTitle,
      description: quizDescription,
    };

    try {
      await axios.post('https://localhost:7280/api/quizzes', newQuiz, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setQuizTitle('');
      setQuizDescription('');
      alert('Quiz created successfully');
    } catch (error) {
      console.error(error);
      alert('Failed to create quiz');
    }
  };

  return (
    <div>
      <h3>Create a new Quiz</h3>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Title:</label>
          <input
            type="text"
            value={quizTitle}
            onChange={(e) => setQuizTitle(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Description:</label>
          <textarea
            value={quizDescription}
            onChange={(e) => setQuizDescription(e.target.value)}
            required
          />
        </div>
        <button type="submit">Create Quiz</button>
      </form>
    </div>
  );
};

export default Quizzes;
