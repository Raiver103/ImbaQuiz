// Answers.jsx
import React, { useState, useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";

const Answers = ({ questionId }) => {
  const { getAccessTokenSilently } = useAuth0();
  const [answers, setAnswers] = useState([]);
  const [newAnswerText, setNewAnswerText] = useState("");
  const [isCorrectAnswer, setIsCorrectAnswer] = useState(false);

  useEffect(() => {
    const fetchAnswers = async () => {
      if (!questionId) return;
      const token = await getAccessTokenSilently();
      try {
        const res = await axios.get(`https://localhost:7280/api/answers/byQuestion/${questionId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setAnswers(res.data);
      } catch (error) {
        console.error("Error fetching answers:", error);
      }
    };
    fetchAnswers();
  }, [getAccessTokenSilently, questionId]);

  const handleAddAnswer = async (e) => {
    e.preventDefault();
    if (!newAnswerText || !questionId) return;

    const token = await getAccessTokenSilently();
    try {
      const res = await axios.post(
        "https://localhost:7280/api/answers",
        { text: newAnswerText, isCorrect: isCorrectAnswer, questionId },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setAnswers([...answers, res.data]);
      setNewAnswerText(""); // Очистим поле ввода
    } catch (error) {
      console.error("Error adding answer:", error);
    }
  };

  const handleDeleteAnswer = async (answerId) => {
    const token = await getAccessTokenSilently();
    try {
      await axios.delete(`https://localhost:7280/api/answers/${answerId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setAnswers(answers.filter((answer) => answer.id !== answerId));
    } catch (error) {
      console.error("Error deleting answer:", error);
    }
  };

  return (
    <div>
      <h3>Answers for Question {questionId}</h3>
      <ul>
        {answers.map((answer) => (
          <li key={answer.id}>
            {answer.text} {answer.isCorrect ? "(Correct)" : "(Incorrect)"}
            <button onClick={() => handleDeleteAnswer(answer.id)}>Delete</button>
          </li>
        ))}
      </ul>

      <form onSubmit={handleAddAnswer}>
        <input
          type="text"
          value={newAnswerText}
          onChange={(e) => setNewAnswerText(e.target.value)}
          placeholder="Enter new answer text"
          required
        />
        <label>
          Correct Answer
          <input
            type="checkbox"
            checked={isCorrectAnswer}
            onChange={() => setIsCorrectAnswer(!isCorrectAnswer)}
          />
        </label>
        <button type="submit">Add Answer</button>
      </form>
    </div>
  );
};

export default Answers;
