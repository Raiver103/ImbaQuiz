import React, { useState, useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";
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
      const token = await getAccessTokenSilently();
      try {
        const res = await axios.get(`https://localhost:7280/api/questions/by-quiz/${quizId}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setQuestions(res.data);
      } catch (error) {
        console.error("Error fetching questions:", error);
      }
    };
    fetchQuestions();
  }, [getAccessTokenSilently, quizId]);

  const handleAddQuestion = async (e) => {
    e.preventDefault();
    if (!newQuestionText) return;

    const token = await getAccessTokenSilently();
    try {
      const res = await axios.post(
        "https://localhost:7280/api/questions",
        { text: newQuestionText, quizId },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setQuestions([...questions, res.data]);
      setNewQuestionText("");
    } catch (error) {
      console.error("Error adding question:", error);
    }
  };

  const handleDeleteQuestion = async (questionId) => {
    const token = await getAccessTokenSilently();
    try {
      await axios.delete(`https://localhost:7280/api/questions/${questionId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
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
    const token = await getAccessTokenSilently();
    try {
      await axios.put(
        `https://localhost:7280/api/questions/${questionId}`,
        { text: editedText, quizId },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setQuestions(
        questions.map((q) => (q.id === questionId ? { ...q, text: editedText } : q))
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