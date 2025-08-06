import React from 'react';
import { NavLink  } from 'react-router-dom';

export default function Sidebar({ userRole, onLogout }) {
  return (
    <div className="h-screen bg-secondaryColor text-white flex flex-col p-6">
      <h2 className="text-2xl font-bold mb-8">{userRole == 'patient' ? <>Patient</> : <>Doctor</>} Account</h2>

      {userRole === 'patient' && (
        <nav className="flex flex-col space-y-4">
          <NavLink to="/profile" className={highlightActive}>Profile</NavLink>
          <NavLink to="/dashboard" className={highlightActive}>Dashboard</NavLink>
          <NavLink to="/appointments" className={highlightActive}>Appointments</NavLink>
          <NavLink to="/video-call" className={highlightActive}>Video Call</NavLink>
        </nav>
      )}

      {userRole === 'doctor' && (
        <nav className="flex flex-col space-y-4">
          <NavLink to="/profile" className={highlightActive}>Profile</NavLink>
          <NavLink to="/dashboard" className={highlightActive}>Dashboard</NavLink>
          <NavLink to="/assessment" className={highlightActive}>Assessment</NavLink>
          <NavLink to="/medical-history" className={highlightActive}>Medical History</NavLink>
          <NavLink to="/prescriptions" className={highlightActive}>Prescriptions</NavLink>
          <NavLink to="/video-call" className={highlightActive}>Video Call</NavLink>
        </nav>
      )}

      <button className="mt-auto text-lg hover:text-gray-300" onClick={onLogout}>Logout</button>
    </div>
  );
}


function highlightActive({isActive})
{
  return isActive ? "text-lg text-blue-400 font-bold" : "text-lg hover:text-gray-300"
}