import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";

const QuizGame = () => {
  const { quizId } = useParams();
  const [questions, setQuestions] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [showAnswer, setShowAnswer] = useState(false);
  const [loading, setLoading] = useState(true);
  const [gameOver, setGameOver] = useState(false);

  useEffect(() => {
    const fetchQuestions = async () => {
      try {
        const response = await axios.get(`https://localhost:7280/api/questions/byQuiz/${quizId}`);
        console.log("Fetched questions:", response.data);

        const questionsWithAnswers = await Promise.all(
          response.data.map(async (question) => {
            try {
              const answersRes = await axios.get(`https://localhost:7280/api/answers/byQuestion/${question.id}`);
              return { ...question, answers: answersRes.data || [] };
            } catch (error) {
              console.error(`Error fetching answers for question ${question.id}:`, error);
              return { ...question, answers: [] };
            }
          })
        );

        console.log("Final questions with answers:", questionsWithAnswers);
        setQuestions(questionsWithAnswers);
      } catch (error) {
        console.error("Error fetching questions:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchQuestions();
  }, [quizId]);

  useEffect(() => {
    if (questions.length === 0 || gameOver) return;
    let timer;

    if (!showAnswer) {
      timer = setTimeout(() => setShowAnswer(true), 5000);
    } else {
      timer = setTimeout(() => {
        if (currentQuestionIndex < questions.length - 1) {
          setCurrentQuestionIndex((prevIndex) => prevIndex + 1);
          setShowAnswer(false);
        } else {
          setGameOver(true);
        }
      }, 2000);
    }

    return () => clearTimeout(timer);
  }, [currentQuestionIndex, showAnswer, questions, gameOver]);

  const restartGame = () => {
    setCurrentQuestionIndex(0);
    setShowAnswer(false);
    setGameOver(false);
  };

  if (loading) return <div>Loading...</div>;
  if (questions.length === 0) return <div>No questions found</div>;
  if (gameOver)
    return (
      <div className="quiz-game">
        <h2>Игра окончена!</h2>
        <button onClick={restartGame}>Начать заново</button>
      </div>
    );

  const currentQuestion = questions[currentQuestionIndex];

  return (
    <div className="quiz-game">
      <h2>Quiz Game</h2>
      <h3>{currentQuestion.text}</h3>
      {!showAnswer ? (
        <ul>
          {currentQuestion.answers.length > 0 ? (
            currentQuestion.answers.map((answer) => <li key={answer.id}>{answer.text}</li>)
          ) : (
            <li>No answers available</li>
          )}
        </ul>
      ) : (
        <div>
          <h4>Correct Answer:</h4>
          <ul>
            {currentQuestion.answers.some((answer) => answer.isCorrect) ? (
              currentQuestion.answers
                .filter((answer) => answer.isCorrect)
                .map((answer) => <li key={answer.id}>{answer.text}</li>)
            ) : (
              <li>No correct answer available</li>
            )}
          </ul>
        </div>
      )}
    </div>
  );
};

export default QuizGame;
