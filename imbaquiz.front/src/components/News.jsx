// components/News.js
import { useAuth0 } from "@auth0/auth0-react";
import React, { useState, useEffect } from 'react';
import { getNews } from '../services/newsApi'; // Импортируем из нового API

const News = () => {
  const { getAccessTokenSilently } = useAuth0(); // вот тут
  const [news, setNews] = useState([]);

  const fetchNews = async () => {
    try {
      const newsData = await getNews(getAccessTokenSilently); // и сюда
      setNews(newsData);
    } catch (error) {
      console.error("Error fetching news:", error);
    }
  };
  
  useEffect(() => {
    fetchNews();
  }, []);

  return (
    <div>
      <h2>Новости</h2>
      <ul>
        {news.map((item, index) => (
          <li key={index}>
            <p>{item}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default News;
