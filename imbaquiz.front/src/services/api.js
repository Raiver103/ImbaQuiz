import axios from "axios";
import { useAuth0 } from "@auth0/auth0-react";

const apiUrl = import.meta.env.VITE_API_URL;

const createAxiosInstance = (getAccessTokenSilently) => {
  const axiosInstance = axios.create({
    baseURL: apiUrl,  
  });
 
  axiosInstance.interceptors.request.use(
    async (config) => {
      const token = await getAccessTokenSilently();  
      if (token) {
        config.headers.Authorization = `Bearer ${token}`; 
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  return axiosInstance;
};

export const getUser = async (userId) => {
  try {
    const response = await axios.get(`${apiUrl}/users/${userId}`);
    return response;
  } catch (error) {
    throw error;
  }
};

export const createUser = async (userData) => {
  try {
    const response = await axios.post(`${apiUrl}/users`, userData);
    return response;
  } catch (error) {
    throw error;
  }
};
 
export const getAnswersByQuestion = async (questionId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.get(`/answers/by-question/${questionId}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const createAnswer = async (newAnswer, questionId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.post("/answers", { ...newAnswer, questionId });
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const deleteAnswer = async (answerId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    await axiosInstance.delete(`/answers/${answerId}`);
  } catch (error) {
    throw error;
  }
};

export const updateAnswer = async (answerId, updatedAnswer, questionId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.put(`/answers/${answerId}`, { ...updatedAnswer, questionId });
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getQuestionsByQuiz = async (quizId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.get(`/questions/by-quiz/${quizId}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const createQuestion = async (newQuestion, quizId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.post("/questions", { ...newQuestion, quizId });
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const deleteQuestion = async (questionId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    await axiosInstance.delete(`/questions/${questionId}`);
  } catch (error) {
    throw error;
  }
};

export const updateQuestion = async (questionId, updatedQuestion, quizId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.put(`/questions/${questionId}`, { ...updatedQuestion, quizId });
    return response.data;
  } catch (error) {
    throw error;
  }
};


export const getQuizzes = async (getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.get('/quizzes');
    console.log(response);
    console.log(response.data);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const createQuiz = async (title, userId, getAccessTokenSilently) => {
  console.log()
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.post("/quizzes", { title, userId });
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const updateQuiz = async (quizId, title, userId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.put(`/quizzes/${quizId}`, { title, userId });
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const deleteQuiz = async (quizId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    await axiosInstance.delete(`/quizzes/${quizId}`);
  } catch (error) {
    throw error;
  }
}; 

export const getAnswersForQuestion = async (questionId, getAccessTokenSilently) => {
  const axiosInstance = createAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.get(`/answers/by-question/${questionId}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getQuestionsWithAnswers = async (quizId, getAccessTokenSilently) => {
  try {
    const questions = await getQuestionsByQuiz(quizId, getAccessTokenSilently);
    
    const questionsWithAnswers = await Promise.all(
      questions.map(async (question) => {
        try {
          const answers = await getAnswersForQuestion(question.id, getAccessTokenSilently);
          return { ...question, answers: answers || [] };
        } catch (error) {
          console.error(`Error fetching answers for question ${question.id}:`, error);
          return { ...question, answers: [] };
        }
      })
    );
    
    return questionsWithAnswers;
  } catch (error) {
    throw error;
  }
};