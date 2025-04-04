// Questions.jsx
import React, { useState, useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { getQuestionsByQuiz, createQuestion, deleteQuestion, updateQuestion } from "../services/api";  // Импортируем функции из api.js
import Answers from "./Answers";

const Questions = ({ quizId }) => {
  const { getAccessTokenSilently } = useAuth0();
  const [questions, setQuestions] = useState([]);
  const [newQuestionText, setNewQuestionText] = useState("");
  const [selectedQuestionId, setSelectedQuestionId] = useState(null);
  const [editingQuestionId, setEditingQuestionId] = useState(null);
  const [editedText, setEditedText] = useState("");

  useEffect(() => {
    const fetchQuestions = async () => {
      if (!quizId) return;
      try {
        const data = await getQuestionsByQuiz(quizId, getAccessTokenSilently);
        setQuestions(data);
      } catch (error) {
        console.error("Error fetching questions:", error);
      }
    };
    fetchQuestions();
  }, [quizId, getAccessTokenSilently]);

  const handleAddQuestion = async (e) => {
    e.preventDefault();
    if (!newQuestionText) return;

    const newQuestion = { text: newQuestionText };
    try {
      const addedQuestion = await createQuestion(newQuestion, quizId, getAccessTokenSilently);
      setQuestions([...questions, addedQuestion]);
      setNewQuestionText("");
    } catch (error) {
      console.error("Error adding question:", error);
    }
  };

  const handleDeleteQuestion = async (questionId) => {
    try {
      await deleteQuestion(questionId, getAccessTokenSilently);
      setQuestions(questions.filter((question) => question.id !== questionId));
    } catch (error) {
      console.error("Error deleting question:", error);
    }
  };

  const handleEditQuestion = (question) => {
    setEditingQuestionId(question.id);
    setEditedText(question.text);
  };

  const handleSaveEdit = async (questionId) => {
    if (!editedText) return;
    try {
      const updated = await updateQuestion(questionId, { text: editedText }, quizId, getAccessTokenSilently);
      setQuestions(
        questions.map((q) => (q.id === questionId ? { ...q, text: updated.text } : q))
      );
      setEditingQuestionId(null);
    } catch (error) {
      console.error("Error updating question:", error);
    }
  };

  return (
    <div>
      <h3>Questions for Quiz {quizId}</h3>
      <ul>
        {questions.map((question) => (
          <li key={question.id}>
            {editingQuestionId === question.id ? (
              <input
                type="text"
                value={editedText}
                onChange={(e) => setEditedText(e.target.value)}
              />
            ) : (
              question.text
            )}
            <button onClick={() => setSelectedQuestionId(question.id)}>
              {selectedQuestionId === question.id ? "Hide Answers" : "View Answers"}
            </button>
            {editingQuestionId === question.id ? (
              <button onClick={() => handleSaveEdit(question.id)}>Save</button>
            ) : (
              <button onClick={() => handleEditQuestion(question)}>Edit</button>
            )}
            <button onClick={() => handleDeleteQuestion(question.id)}>Delete</button>
            {selectedQuestionId === question.id && <Answers questionId={question.id} />}
          </li>
        ))}
      </ul>
      <form onSubmit={handleAddQuestion}>
        <input
          type="text"
          value={newQuestionText}
          onChange={(e) => setNewQuestionText(e.target.value)}
          placeholder="Enter new question text"
          required
        />
        <button type="submit">Add Question</button>
      </form>
    </div>
  );
};

export default Questions;
