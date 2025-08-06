import React from 'react';
import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';

export default function ProtectedLayout({ userRole, onLogout }) {
    return (
      <div className="flex h-screen">
        <Sidebar userRole={userRole} onLogout={onLogout} />
        <div className="w-screen p-8 bg-gray-100">
          <Outlet />
        </div>
      </div>
    );
  }
