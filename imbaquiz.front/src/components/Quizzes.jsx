import React, { useState, useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Link } from "react-router-dom";
import Questions from "./Questions";
import { getPaginatedQuizzes, createQuiz, updateQuiz, deleteQuiz } from "../services/api";

const Quizzes = () => {
  const { getAccessTokenSilently, user } = useAuth0();
  const [quizzes, setQuizzes] = useState([]);
  const [selectedQuizId, setSelectedQuizId] = useState(null);
  const [quizTitle, setQuizTitle] = useState("");
  const [editQuizId, setEditQuizId] = useState(null);
  const [editQuizTitle, setEditQuizTitle] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(4);
  const [totalPages, setTotalPages] = useState(1);

  const fetchPaginatedQuizzes = async () => {
    if (!user) return;
    try {
      const result = await getPaginatedQuizzes(currentPage, pageSize, getAccessTokenSilently, user.sub);
  
      const userQuizzes = result.items.filter((quiz) => quiz.userId === user.sub);
      setQuizzes(userQuizzes);
  
      if (!isNaN(result.totalCount)) {
        const pages = Math.ceil(result.totalCount / pageSize);
        setTotalPages(pages);
      } else {
        console.warn("Invalid totalCount:", result.totalCount);
        setTotalPages(1);
      }
    } catch (error) {
      console.error("Error fetching paginated quizzes:", error);
    }
  };
  

  useEffect(() => {
    fetchPaginatedQuizzes();
  }, [currentPage, pageSize, getAccessTokenSilently, user]);

  const handleAddQuiz = async (e) => {
    e.preventDefault();
    try {
      const newQuiz = await createQuiz(quizTitle, user.sub, getAccessTokenSilently);
      setQuizTitle("");
      fetchPaginatedQuizzes();
    } catch (error) {
      console.error("Error adding quiz:", error);
    }
  };

  const handleUpdateQuiz = async () => {
    if (!editQuizTitle) return;
    try {
      await updateQuiz(editQuizId, editQuizTitle, user.sub, getAccessTokenSilently);
      fetchPaginatedQuizzes();
      setEditQuizId(null);
      setEditQuizTitle("");
    } catch (error) {
      console.error("Error updating quiz:", error);
    }
  };

  const handleDeleteQuiz = async (quizId) => {
    try {
      await deleteQuiz(quizId, getAccessTokenSilently);
      fetchPaginatedQuizzes();
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

      <div style={{ marginTop: '20px' }}>
        <label>
          Count of pages: 
          <select
            value={pageSize}
            onChange={(e) => {
              setCurrentPage(1);
              setPageSize(Number(e.target.value));
            }}
          >
            <option value={2}>2</option>
            <option value={4}>4</option>
            <option value={10}>10</option>
          </select>
        </label>

        <div>
          <button
            onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}
            disabled={currentPage === 1}
          >
            Back
          </button>

          <span style={{ margin: '0 10px' }}>
            Page {currentPage}/{totalPages}
          </span>

          <button
            onClick={() => setCurrentPage(prev => Math.min(prev + 1, totalPages))}
            disabled={currentPage === totalPages}
          >
            Next
          </button>
        </div>
      </div>
    </div>
  );
};

export default Quizzes;
