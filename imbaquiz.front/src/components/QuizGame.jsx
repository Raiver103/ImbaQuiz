import React, { useEffect, useState, useRef } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import "../styles/QuizGame.css";

const QuizGame = () => {
  const { quizId } = useParams();
  const [questions, setQuestions] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [selectedAnswer, setSelectedAnswer] = useState(null);
  const [showAnswer, setShowAnswer] = useState(false);
  const [score, setScore] = useState(0);
  const [timeLeft, setTimeLeft] = useState(10);
  const [gameOver, setGameOver] = useState(false);
  const [started, setStarted] = useState(false);
  
  const timerRef = useRef(null);
  const isTransitioningRef = useRef(false);  
 
  useEffect(() => {
    const fetchQuestions = async () => {
      try {
        const response = await axios.get(`https://localhost:7280/api/questions/by-quiz/${quizId}`);
        const questionsWithAnswers = await Promise.all(
          response.data.map(async (question) => {
            try {
              const answersRes = await axios.get(`https://localhost:7280/api/answers/by-question/${question.id}`);
              return { ...question, answers: answersRes.data || [] };
            } catch (error) {
              console.error(`Error fetching answers for question ${question.id}:`, error);
              return { ...question, answers: [] };
            }
          })
        );
        setQuestions(questionsWithAnswers);
      } catch (error) {
        console.error("Error fetching questions:", error);
      }
    };
    fetchQuestions();
  }, [quizId]);
 
  const clearTimer = () => {
    if (timerRef.current) {
      clearInterval(timerRef.current);
      timerRef.current = null;
    }
  };
 
  const handleNextQuestion = () => {
    if (isTransitioningRef.current) return;
    isTransitioningRef.current = true;
    
    clearTimer();
    
    if (currentQuestionIndex < questions.length - 1) {
      setCurrentQuestionIndex(prev => prev + 1);
      setSelectedAnswer(null);
      setShowAnswer(false);
      setTimeLeft(10);
    } else {
      setGameOver(true);
    }
     
    setTimeout(() => {
      isTransitioningRef.current = false;
    }, 0);
  };
 
  const startTimer = () => {
    clearTimer();
    setTimeLeft(10);
    
    timerRef.current = setInterval(() => {
      setTimeLeft(prev => {
        if (prev <= 1) {
          handleNextQuestion();
          return 0;
        }
        return prev - 1;
      });
    }, 1000);
  };
 
  const handleAnswerClick = (answer) => {
    if (!showAnswer && !isTransitioningRef.current) {
      clearTimer();
      setSelectedAnswer(answer);
      if (answer.isCorrect) {
        setScore(prev => prev + 1);
      }
      setShowAnswer(true);
       
      setTimeout(handleNextQuestion, 2000);
    }
  };
 
  useEffect(() => {
    if (started && !gameOver && !showAnswer && questions.length > 0) {
      startTimer();
    }
    
    return () => clearTimer();
  }, [started, gameOver, showAnswer, currentQuestionIndex, questions.length]);
 
  const restartGame = () => {
    clearTimer();
    isTransitioningRef.current = false;
    setCurrentQuestionIndex(0);
    setSelectedAnswer(null);
    setShowAnswer(false);
    setScore(0);
    setTimeLeft(10);
    setGameOver(false);
    setStarted(false);
  };

  if (!started) {
    return (
      <div className="quiz-container">
        <h2>Quiz Game</h2>
        <button className="start-button" onClick={() => setStarted(true)}>
          Начать викторину
        </button>
      </div>
    );
  }

  if (gameOver) {
    return (
      <div className="quiz-container">
        <h2>Игра окончена!</h2>
        <p className="score-text">
          Ваш счет: <span className="score-value">{score}</span> из <span className="total-questions">{questions.length}</span>
        </p>
        <button className="restart-button" onClick={restartGame}>
          Начать заново
        </button>
      </div>
    );
  }

  if (questions.length === 0) {
    return (
      <div className="quiz-container">
        <h2>Загрузка вопросов...</h2>
      </div>
    );
  }

  const currentQuestion = questions[currentQuestionIndex];

  return (
    <div className="quiz-container">
      <div className="quiz-header">
        <h2>Вопрос {currentQuestionIndex + 1} из {questions.length}</h2>
        <div className="timer">⏱️ {timeLeft} сек</div>
      </div>
      
      <div className="question-card">
        <h3 className="question-text">{currentQuestion.text}</h3>
        
        <div className="answers-container">
          {currentQuestion.answers.map((answer) => {
            let buttonClass = "answer-button";
            
            if (showAnswer) {
              if (answer.isCorrect) {
                buttonClass += " correct-answer";
              } else if (selectedAnswer?.id === answer.id && !answer.isCorrect) {
                buttonClass += " wrong-answer";
              }
            }

            return (
              <button
                key={answer.id}
                className={buttonClass}
                onClick={() => handleAnswerClick(answer)}
                disabled={showAnswer || isTransitioningRef.current}
              >
                {answer.text}
              </button>
            );
          })}
        </div>
      </div>

      {showAnswer && (
        <div className="feedback">
          {selectedAnswer?.isCorrect ? (
            <p className="correct-feedback">Правильно! ✅</p>
          ) : (
            <p className="wrong-feedback">Неправильно! ❌</p>
          )}
        </div>
      )}
    </div>
  );
};

export default QuizGame;