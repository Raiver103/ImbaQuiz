import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";

const QuizGame = () => {
  const { quizId } = useParams();
  const navigate = useNavigate();
  const { getAccessTokenSilently, user } = useAuth0();
  const [questions, setQuestions] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [timer, setTimer] = useState(5);
  const [gameState, setGameState] = useState("pre-game"); // 'pre-game', 'playing', 'finished'
  const [score, setScore] = useState(0);
  const [selectedAnswer, setSelectedAnswer] = useState(null);
  const [quizTitle, setQuizTitle] = useState("");

  // Получаем вопросы и информацию о квизе
  useEffect(() => {
    const fetchData = async () => {
      const token = await getAccessTokenSilently();
      try {
        // Получаем вопросы
        const questionsRes = await axios.get(
          `https://localhost:7280/api/Questions/ByQuizId/${quizId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        
        // Получаем название квиза
        const quizRes = await axios.get(
          `https://localhost:7280/api/quizzes/${quizId}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        
        setQuestions(questionsRes.data);
        setQuizTitle(quizRes.data.title || "Без названия");
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };
    
    fetchData();
  }, [quizId, getAccessTokenSilently]);

  // Таймер для вопросов
  useEffect(() => {
    let interval;
    if (gameState === 'playing' && questions.length > 0) {
      interval = setInterval(() => {
        setTimer(prev => {
          if (prev <= 1) {
            // Переход к следующему вопросу или завершение игры
            if (currentQuestionIndex < questions.length - 1) {
              nextQuestion();
              return 5;
            } else {
              endGame();
              clearInterval(interval);
              return 0;
            }
          }
          return prev - 1;
        });
      }, 1000);
    }
    return () => clearInterval(interval);
  }, [gameState, currentQuestionIndex, questions.length]);

  const startGame = () => {
    setGameState('playing');
    setCurrentQuestionIndex(0);
    setTimer(5);
    setScore(0);
    setSelectedAnswer(null);
  };

  const nextQuestion = () => {
    setCurrentQuestionIndex(prev => prev + 1);
    setTimer(5);
    setSelectedAnswer(null);
  };

  const endGame = () => {
    setGameState('finished');
  };

  const handleAnswerSelect = (answer) => {
    if (selectedAnswer || gameState !== 'playing') return;
    
    setSelectedAnswer(answer);
    if (answer.isCorrect) {
      setScore(prev => prev + 1);
    }
  };

  const restartGame = () => {
    startGame();
  };

  if (questions.length === 0) {
    return (
      <div className="quiz-game-container">
        <h2>Загрузка вопросов...</h2>
        <button onClick={() => navigate(-1)}>Назад к квизам</button>
      </div>
    );
  }

  if (gameState === 'pre-game') {
    return (
      <div className="quiz-game-container">
        <h2>{quizTitle}</h2>
        <p>Количество вопросов: {questions.length}</p>
        <button onClick={startGame}>Начать игру</button>
        <button onClick={() => navigate(-1)}>Назад к квизам</button>
      </div>
    );
  }

  if (gameState === 'finished') {
    return (
      <div className="quiz-game-container">
        <h2>Игра завершена!</h2>
        <p>Ваш результат: {score} из {questions.length}</p>
        <button onClick={restartGame}>Играть снова</button>
        <button onClick={() => navigate(-1)}>Назад к квизам</button>
      </div>
    );
  }

  const currentQuestion = questions[currentQuestionIndex];

  return (
    <div className="quiz-game-container">
      <div className="quiz-header">
        <h2>{quizTitle}</h2>
        <div className="quiz-info">
          <span>Вопрос {currentQuestionIndex + 1} из {questions.length}</span>
          <span>Осталось: {timer} сек</span>
          <span>Счет: {score}</span>
        </div>
      </div>
      
      <div className="question-container">
        <h3>{currentQuestion.text}</h3>
        
        <div className="answers-grid">
          {currentQuestion.answers && currentQuestion.answers.map((answer) => (
            <button
              key={answer.id}
              className={`answer-button 
                ${selectedAnswer?.id === answer.id ? 
                  (answer.isCorrect ? 'correct' : 'incorrect') : ''}
                ${selectedAnswer && answer.isCorrect ? 'show-correct' : ''}
              `}
              onClick={() => handleAnswerSelect(answer)}
              disabled={!!selectedAnswer}
            >
              {answer.text}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};

export default QuizGame;