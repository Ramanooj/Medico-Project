// src/pages/LoginPage.jsx
import React, { useEffect } from 'react';

export default function LoginPage() {
  useEffect(() => {
    window.location.href = `https://medico.auth.ca-central-1.amazoncognito.com/login?client_id=7dkml64cnved99vu10trq6omu7&response_type=token&scope=aws.cognito.signin.user.admin+email+openid+phone+profile&redirect_uri=${window.location.origin}/auth-redirect`;
  }, []);

  return <div>Redirecting to login...</div>;
}
