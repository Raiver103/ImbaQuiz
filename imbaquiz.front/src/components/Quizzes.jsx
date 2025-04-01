import React, { useState, useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";
import { Link } from "react-router-dom";
import Questions from "./Questions";

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
      const token = await getAccessTokenSilently();
      try {
        const res = await axios.get("https://localhost:7280/api/quizzes", {
          headers: { Authorization: `Bearer ${token}` },
        });
        const userQuizzes = res.data.filter((quiz) => quiz.userId === user.sub);
        setQuizzes(userQuizzes);
      } catch (error) {
        console.error("Error fetching quizzes:", error);
      }
    };
    fetchQuizzes();
  }, [getAccessTokenSilently, user]);

  const handleAddQuiz = async (e) => {
    e.preventDefault();
    const token = await getAccessTokenSilently();
    try {
      const res = await axios.post(
        "https://localhost:7280/api/quizzes",
        { title: quizTitle, userId: user.sub },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setQuizzes([...quizzes, res.data]);
      setQuizTitle("");
    } catch (error) {
      console.error("Error adding quiz:", error);
    }
  };

  const handleUpdateQuiz = async () => {
    if (!editQuizTitle) return;
    const token = await getAccessTokenSilently();
    try {
      await axios.put(
        `https://localhost:7280/api/quizzes/${editQuizId}`,
        { id: editQuizId, title: editQuizTitle, userId: user.sub },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setQuizzes(quizzes.map(q => q.id === editQuizId ? { ...q, title: editQuizTitle } : q));
      setEditQuizId(null);
      setEditQuizTitle("");
    } catch (error) {
      console.error("Error updating quiz:", error);
    }
  };

  const handleDeleteQuiz = async (quizId) => {
    const token = await getAccessTokenSilently();
    try {
      await axios.delete(`https://localhost:7280/api/quizzes/${quizId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
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
                <button onClick={() => { setEditQuizId(quiz.id); setEditQuizTitle(quiz.title); }}>Edit</button>
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
