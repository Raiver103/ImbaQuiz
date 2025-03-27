// src/components/Login.js
import React from 'react';
import { useAuth0 } from '@auth0/auth0-react';

const Login = () => {
  const { loginWithRedirect } = useAuth0();

  return (
    <div>
      <h2>Login to your account</h2>
      <button onClick={() => loginWithRedirect()}>Log in with Auth0</button>
    </div>
  );
};

export default Login;
