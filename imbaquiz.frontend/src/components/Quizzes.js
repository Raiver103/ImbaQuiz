import React, { useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import axios from 'axios';

const Quizzes = () => {
  const { getAccessTokenSilently, user } = useAuth0();
  const [quizTitle, setQuizTitle] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();

    const token = await getAccessTokenSilently(); // Получаем токен
    console.log(token);
    const newQuiz = {
      title: quizTitle,
      userId: user?.sub,
      
    };
    console.log(user?.sub);
    try {
      // Отправляем запрос с токеном
      const res = await axios.post('https://localhost:7280/api/quizzes', newQuiz, {
        headers: {
          Authorization: `Bearer ${token}`, // Токен в заголовке
        },
      });
      setQuizTitle('');
      alert('Quiz created successfully');
    } catch (error) {
      // Выводим подробности ошибки
      console.error("Error details:", error);
      alert(`Failed to create quiz: ${error.response ? JSON.stringify(error.response.data) : error.message}`);
      alert(`Failed to create quiz: ${token}`);
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
        <button type="submit">Create Quiz</button>
      </form>
    </div>
  );
};

export default Quizzes;
