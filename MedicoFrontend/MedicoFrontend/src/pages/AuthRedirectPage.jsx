import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode'; 

export default function AuthRedirectPage({ onTokenExtracted, setUserRole }) {
  const navigate = useNavigate();

  useEffect(() => {
    const hash = window.location.hash;
  
    if (hash) {
      const params = new URLSearchParams(hash.replace('#', '?'));
      const accessToken = params.get('access_token');
      const idToken = params.get('id_token'); 
      const expiry = params.get('expires_in');
  
      try {
        if (accessToken && idToken && expiry) {
          const decodedAccessToken = jwtDecode(accessToken);
          const userRole = decodedAccessToken['cognito:groups'] ? decodedAccessToken['cognito:groups'][0] : null;
  
          const expiryTime = Date.now() + parseInt(expiry, 10) * 1000;
          
          onTokenExtracted(accessToken);
          setUserRole(userRole);
          localStorage.setItem('accessToken', accessToken);
          localStorage.setItem('idToken', idToken);
          localStorage.setItem('expiryTime', expiryTime);
          localStorage.setItem('userRole', userRole);
          localStorage.setItem('baseUrl', 'https://projectmedico.site/api')
  
          window.history.replaceState({}, document.title, window.location.pathname);

          checkUserRegistration(accessToken, userRole);
        }
      } catch (e) {
        console.error('Failed to decode token', e);
      }
    }
  }, [navigate, onTokenExtracted]);

  const checkUserRegistration = async (token, userRole) => {
    try {
      let completeUrl = `${localStorage.getItem('baseUrl')}`;
      if(userRole === 'patient')
      {
        completeUrl += "/Patients/isPatientRegistered"
      }
      if(userRole === 'doctor')
      {
        completeUrl += "/Doctors/isDoctorRegistered"
      }
      const response = await fetch(completeUrl, {
        method: 'GET',
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });
      const data = await response.json();

      if (data.isRegistered) {
        navigate('/dashboard');
      } else {
        if (userRole === 'patient') {
          navigate('/register-patient');
        } else if (userRole === 'doctor') {
          navigate('/register-doctor');
        }
      }
    } catch (error) {
      console.error('Error checking registration status:', error);
    }
  };

  return <div>Processing authentication...</div>;
}
