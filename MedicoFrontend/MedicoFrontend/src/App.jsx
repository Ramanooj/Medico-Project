import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import ProtectedLayout from './components/layout/ProtectedLayout';
import LandingPage from './pages/LandingPage';
import LoginPage from './pages/LoginPage';
import DashboardPage from './pages/DashboardPage';
import AuthRedirectPage from './pages/AuthRedirectPage';
import AppointmentPage from "./pages/AppointmentPage";
import AssessmentPage from "./pages/AssessmentPage";
import MedicalHistoryPage from "./pages/MedicalHistoryPage";
import PrescriptionPage from "./pages/PresciptionPage";
import ProfilePage from './pages/ProfilePage';
import PreVideoCallPage from './pages/videocall/PreVideoCallPage'
import ForbiddenPage from './pages/errorpages/ForbiddenPage';
import NotFoundPage from './pages/errorpages/NotFoundPage';
import SignupPage from './pages/SignupPage';
import RegisterPatientFormPage from './pages/RegisterPatientFormPage';
import RegisterDoctorFormPage from './pages/RegisterDoctorPage';
import Footer from './components/reusable/Footer'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';


export default function App() {
  const [accessToken, setAccessToken] = useState(localStorage.getItem('accessToken'));
  const [userRole, setUserRole] = useState(localStorage.getItem('userRole'));
  const [isTokenChecked, setIsTokenChecked] = useState(false);
  const reactClient = new QueryClient();


  useEffect(() => {
    const storedExpiryTime = localStorage.getItem('expiryTime');
    if (storedExpiryTime && Date.now() > storedExpiryTime) {
      handleLogout();
    }
    else {
      if(accessToken)
      {
        setIsTokenChecked(true);
      }
    }
  });

  useEffect(() => {
    const handleStorageChange = (e) => {
      if (e.key === 'accessToken' && e.newValue === null) {
        setAccessToken(null);
        setUserRole(null);
      }
    };

    window.addEventListener('storage', handleStorageChange);
    return () => window.removeEventListener('storage', handleStorageChange);
  }, []);

  const handleLogout = () => {
    setAccessToken(null);
    localStorage.removeItem('accessToken');
    localStorage.removeItem('expiryTime');
    localStorage.removeItem('userRole');
    localStorage.removeItem('idToken');
    localStorage.removeItem('baseUrl');
    window.location.href = `https://medico.auth.ca-central-1.amazoncognito.com/logout?client_id=7dkml64cnved99vu10trq6omu7&logout_uri=${window.location.origin}`;
  };

  return (
    <QueryClientProvider client={reactClient}>
      <Router>
            <Routes>
              <Route path="/" element={<LandingPage />} />
              <Route path="/login" element={<LoginPage />} />
              <Route path="/signup" element={<SignupPage />}/>
              

              <Route path="/auth-redirect" element={<AuthRedirectPage onTokenExtracted={setAccessToken} setUserRole={setUserRole}/>} />
              <Route path="/forbidden" element={<ForbiddenPage onLogout={handleLogout} />} />
              <Route path="/register-patient" element={userRole === 'patient' ? <RegisterPatientFormPage/> : <Navigate to="/forbidden" />} />
              <Route path="/register-doctor" element={userRole === 'doctor' ? <RegisterDoctorFormPage/> : <Navigate to="/forbidden" />} />

              <Route element={{isTokenChecked} && <ProtectedLayout userRole={userRole} onLogout={handleLogout} />}>
                <Route path="/dashboard" element={<DashboardPage />} />
                <Route path="/video-call" element={ <PreVideoCallPage />}/>
                <Route path="/profile" element={userRole ? <ProfilePage userRole={userRole} /> : <Navigate to="/forbidden" />} />
                <Route path="/appointments" element={userRole === 'patient' ? <AppointmentPage /> : <Navigate to="/forbidden" />} />
                <Route path="/assessment" element={userRole === 'doctor' ? <AssessmentPage /> : <Navigate to="/forbidden" />} />
                <Route path="/medical-history" element={userRole === 'doctor' ? <MedicalHistoryPage /> : <Navigate to="/forbidden" />} />
                <Route path="/prescriptions" element={userRole === 'doctor'? <PrescriptionPage /> : <Navigate to="/forbidden" />} />
              </Route>
              <Route path="*" element={<NotFoundPage />} />
            </Routes>
          <Footer /> 
      </Router>
    </QueryClientProvider>
  );
}