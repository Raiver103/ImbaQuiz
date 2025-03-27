// src/App.js
import React from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import './App.css';

function App() {
  const { loginWithRedirect, logout, user, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="App">
      <header className="App-header">
        <h1>Welcome to Imba Quiz</h1>
        {!isAuthenticated ? (
          <button onClick={() => loginWithRedirect()}>Login with Auth0</button>
        ) : (
          <div>
            <h2>Hello, {user?.name}</h2>
            <p>Email: {user?.email}</p>
            <button onClick={() => logout({ returnTo: window.location.origin })}>
              Logout
            </button>
          </div>
        )}
      </header>
    </div>
  );
}

export default App;
