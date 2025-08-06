import React from "react";
import { useNavigate } from "react-router-dom";
import LogoutButton from "../../components/logout/LogoutButton";

function ForbiddenPage({onLogout}) {
    
    const navigate = useNavigate();
    const handleDashboardReroute = () => {
        navigate("/dashboard");
      };

    return (
        <div>
            <h1>You don't have access to this page!</h1>
            <button onClick={handleDashboardReroute}>Home</button>
            {<LogoutButton onLogout={onLogout} />}
        </div>
    );
}

export default ForbiddenPage;