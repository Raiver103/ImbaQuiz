import React, { useState, useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Link } from "react-router-dom";
import Questions from "./Questions";
import {
  getQuizzes,
  createQuiz,
  updateQuiz,
  deleteQuiz,
} from "../services/api";

const Quizzes = () => {
  const { getAccessTokenSilently, user } = useAuth0();
  const [quizzes, setQuizzes] = useState([]);
  const [selectedQuizId, setSelectedQuizId] = useState(null);
  const [quizTitle, setQuizTitle] = useState("");
  const [editQuizId, setEditQuizId] = useState(null);
  const [editQuizTitle, setEditQuizTitle] = useState("");

  useEffect(() => {
    const fetchQuizzes = async () => {
      if (!user) return;
      try {
        const allQuizzes = await getQuizzes(getAccessTokenSilently);
        const userQuizzes = allQuizzes.filter((quiz) => quiz.userId === user.sub);
        setQuizzes(userQuizzes);
      } catch (error) {
        console.error("Error fetching quizzes:", error);
      }
    };
    fetchQuizzes();
  }, [getAccessTokenSilently, user]);

  const handleAddQuiz = async (e) => {
    e.preventDefault();
    try {
      const newQuiz = await createQuiz(quizTitle, user.sub, getAccessTokenSilently);
      setQuizzes([...quizzes, newQuiz]);
      setQuizTitle("");
    } catch (error) {
      console.error("Error adding quiz:", error);
    }
  };

  const handleUpdateQuiz = async () => {
    if (!editQuizTitle) return;
    try {
      await updateQuiz(editQuizId, editQuizTitle, user.sub, getAccessTokenSilently);
      setQuizzes(quizzes.map(q => q.id === editQuizId ? { ...q, title: editQuizTitle } : q));
      setEditQuizId(null);
      setEditQuizTitle("");
    } catch (error) {
      console.error("Error updating quiz:", error);
    }
  };

  const handleDeleteQuiz = async (quizId) => {
    try {
      await deleteQuiz(quizId, getAccessTokenSilently);
      setQuizzes(quizzes.filter((quiz) => quiz.id !== quizId));
      if (selectedQuizId === quizId) {
        setSelectedQuizId(null);
      }
    } catch (error) {
      console.error("Error deleting quiz:", error);
    }
  };

  return (
    <div>
      <h3>Your Quizzes</h3>
      <ul>
        {quizzes.map((quiz) => (
          <li key={quiz.id}>
            {editQuizId === quiz.id ? (
              <>
                <input
                  type="text"
                  value={editQuizTitle}
                  onChange={(e) => setEditQuizTitle(e.target.value)}
                  placeholder="Enter new quiz title"
                  required
                />
                <button onClick={handleUpdateQuiz}>Save</button>
                <button onClick={() => setEditQuizId(null)}>Cancel</button>
              </>
            ) : (
              <>
                {quiz.title}
                <button onClick={() => setSelectedQuizId(quiz.id)}>View</button>
                <button onClick={() => {
                  setEditQuizId(quiz.id);
                  setEditQuizTitle(quiz.title);
                }}>Edit</button>
                <button onClick={() => handleDeleteQuiz(quiz.id)}>Delete</button>
                <Link to={`/quiz-game/${quiz.id}`}>
                  <button style={{ marginLeft: "10px" }}>Играть</button>
                </Link>
              </>
            )}
          </li>
        ))}
      </ul>

      <form onSubmit={handleAddQuiz}>
        <input
          type="text"
          value={quizTitle}
          onChange={(e) => setQuizTitle(e.target.value)}
          placeholder="Enter quiz title"
          required
        />
        <button type="submit">Add Quiz</button>
      </form>

      {selectedQuizId && <Questions quizId={selectedQuizId} />}
    </div>
  );
};

export default Quizzes;
