import React, { useState, useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { getAnswersByQuestion, createAnswer, deleteAnswer, updateAnswer } from "../services/api";   

const Answers = ({ questionId }) => {
  const { getAccessTokenSilently } = useAuth0();
  const [answers, setAnswers] = useState([]);
  const [newAnswerText, setNewAnswerText] = useState("");
  const [isCorrectAnswer, setIsCorrectAnswer] = useState(false);
  const [editingAnswer, setEditingAnswer] = useState(null);
  const [editedText, setEditedText] = useState("");
  const [editedCorrect, setEditedCorrect] = useState(false);

  useEffect(() => {
    const fetchAnswers = async () => {
      if (!questionId) return;
      try {
        const data = await getAnswersByQuestion(questionId, getAccessTokenSilently);
        setAnswers(data);
      } catch (error) {
        console.error("Error fetching answers:", error);
      }
    };
    fetchAnswers();
  }, [questionId, getAccessTokenSilently]);

  const handleAddAnswer = async (e) => {
    e.preventDefault();
    if (!newAnswerText || !questionId) return;

    const newAnswer = { text: newAnswerText, isCorrect: isCorrectAnswer };
    try {
      const addedAnswer = await createAnswer(newAnswer, questionId, getAccessTokenSilently);
      setAnswers([...answers, addedAnswer]);
      setNewAnswerText("");
      setIsCorrectAnswer(false);
    } catch (error) {
      console.error("Error adding answer:", error);
    }
  };

  const handleDeleteAnswer = async (answerId) => {
    try {
      await deleteAnswer(answerId, getAccessTokenSilently);
      setAnswers(answers.filter((answer) => answer.id !== answerId));
    } catch (error) {
      console.error("Error deleting answer:", error);
    }
  };

  const handleEditAnswer = (answer) => {
    setEditingAnswer(answer.id);
    setEditedText(answer.text);
    setEditedCorrect(answer.isCorrect);
  };

  const handleUpdateAnswer = async (e) => {
    e.preventDefault();
    if (!editingAnswer) return;

    const updatedAnswer = { text: editedText, isCorrect: editedCorrect };
    try {
      const updated = await updateAnswer(editingAnswer, updatedAnswer, questionId, getAccessTokenSilently);
      setAnswers(
        answers.map((answer) =>
          answer.id === editingAnswer ? { ...answer, text: updated.text, isCorrect: updated.isCorrect } : answer
        )
      );
      setEditingAnswer(null);
      setEditedText("");
      setEditedCorrect(false);
    } catch (error) {
      console.error("Error updating answer:", error);
    }
  };

  return (
    <div>
      <h3>Answers for Question {questionId}</h3>
      <ul>
        {answers.map((answer) => (
          <li key={answer.id}>
            {editingAnswer === answer.id ? (
              <form onSubmit={handleUpdateAnswer}>
                <input
                  type="text"
                  value={editedText}
                  onChange={(e) => setEditedText(e.target.value)}
                  required
                />
                <label>
                  Correct Answer
                  <input
                    type="checkbox"
                    checked={editedCorrect}
                    onChange={() => setEditedCorrect(!editedCorrect)}
                  />
                </label>
                <button type="submit">Save</button>
                <button type="button" onClick={() => setEditingAnswer(null)}>Cancel</button>
              </form>
            ) : (
              <>
                {answer.text} {answer.isCorrect ? "(Correct)" : "(Incorrect)"}
                <button onClick={() => handleEditAnswer(answer)}>Edit</button>
                <button onClick={() => handleDeleteAnswer(answer.id)}>Delete</button>
              </>
            )}
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
