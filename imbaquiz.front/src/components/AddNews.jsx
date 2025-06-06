import React, { useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { createNews } from '../services/newsApi';

const AddNews = () => {
  const [newsContent, setNewsContent] = useState("");
  const { getAccessTokenSilently } = useAuth0();

  const handleAddNews = async (e) => {
    e.preventDefault();
    try {
      await createNews(newsContent, getAccessTokenSilently); 
      setNewsContent(""); 
    } catch (error) {
      console.error("Error adding news:", error); 
    }
  };

  return (
    <div>
      <h2>Добавить новость</h2>
      <form onSubmit={handleAddNews}>
        <textarea
          value={newsContent}
          onChange={(e) => setNewsContent(e.target.value)}
          placeholder="Введите новость"
          required
        />
        <button type="submit">Добавить</button>
      </form>
    </div>
  );
};

export default AddNews;
