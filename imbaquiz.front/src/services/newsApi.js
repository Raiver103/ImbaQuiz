import axios from "axios";

const newsApiUrl = import.meta.env.VITE_NEWS_API_URL || "http://localhost:5100/api"; 

const createNewsAxiosInstance = (getAccessTokenSilently) => {
  const axiosInstance = axios.create({
    baseURL: newsApiUrl,  
    headers: {
      'Content-Type': 'application/json',  
    }
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
 

export const getNews = async (getAccessTokenSilently) => {
  const axiosInstance = createNewsAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.get('/news');
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const createNews = async (newsContent, getAccessTokenSilently) => {
  const axiosInstance = createNewsAxiosInstance(getAccessTokenSilently);
  try {
    const response = await axiosInstance.post('/news', { news: newsContent }); 
    return response.data;
  } catch (error) {
    throw error;
  }
};