import React, { useEffect } from 'react';

export default function SignupPage() {
  useEffect(() => {
    window.location.href = `https://medico.auth.ca-central-1.amazoncognito.com/signup?client_id=7dkml64cnved99vu10trq6omu7&response_type=token&scope=aws.cognito.signin.user.admin+email+openid+phone+profile&redirect_uri=${window.location.origin}/auth-redirect`;
  }, []);

  return <div>Redirecting to singup</div>;
}
