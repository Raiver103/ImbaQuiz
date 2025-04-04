import React, { useEffect, useState, useRef } from "react";
import { useParams } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { getQuestionsWithAnswers } from "../services/api";
import "../styles/QuizGame.css";

const QuizGame = () => {
  const { quizId } = useParams();
  const { getAccessTokenSilently } = useAuth0();
  const [questions, setQuestions] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [selectedAnswer, setSelectedAnswer] = useState(null);
  const [showAnswer, setShowAnswer] = useState(false);
  const [score, setScore] = useState(0);
  const [timeLeft, setTimeLeft] = useState(10);
  const [gameOver, setGameOver] = useState(false);
  const [started, setStarted] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const timerRef = useRef(null);
  const isTransitioningRef = useRef(false);  
 
  useEffect(() => {
    const fetchQuestions = async () => {
      try {
        setLoading(true);
        const questionsWithAnswers = await getQuestionsWithAnswers(quizId, getAccessTokenSilently);
        setQuestions(questionsWithAnswers);
        setError(null);
      } catch (err) {
        console.error("Error fetching questions:", err);
        setError("Failed to load questions. Please try again later.");
      } finally {
        setLoading(false);
      }
    };
    
    fetchQuestions();
  }, [quizId, getAccessTokenSilently]);

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
        {error ? (
          <div className="error-message">
            <p>{error}</p>
            <button onClick={() => window.location.reload()}>Retry</button>
          </div>
        ) : (
          <button 
            className="start-button" 
            onClick={() => setStarted(true)}
            disabled={loading}
          >
            {loading ? "Loading..." : "Start Quiz"}
          </button>
        )}
      </div>
    );
  }

  if (gameOver) {
    return (
      <div className="quiz-container">
        <h2>Game Over!</h2>
        <p className="score-text">
          Your score: <span className="score-value">{score}</span> out of <span className="total-questions">{questions.length}</span>
        </p>
        <button className="restart-button" onClick={restartGame}>
          Play Again
        </button>
      </div>
    );
  }

  if (loading) {
    return (
      <div className="quiz-container">
        <h2>Loading questions...</h2>
        <div className="loading-spinner"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="quiz-container">
        <h2>Error</h2>
        <p className="error-message">{error}</p>
        <button onClick={() => window.location.reload()}>Retry</button>
      </div>
    );
  }

  if (questions.length === 0) {
    return (
      <div className="quiz-container">
        <h2>No questions available</h2>
        <p>This quiz doesn't have any questions yet.</p>
      </div>
    );
  }

  const currentQuestion = questions[currentQuestionIndex];

  return (
    <div className="quiz-container">
      <div className="quiz-header">
        <h2>Question {currentQuestionIndex + 1} of {questions.length}</h2>
        <div className="timer">⏱️ {timeLeft} sec</div>
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
            <p className="correct-feedback">Correct! ✅</p>
          ) : (
            <p className="wrong-feedback">Wrong! ❌</p>
          )}
        </div>
      )}
    </div>
  );
};

export default QuizGame;