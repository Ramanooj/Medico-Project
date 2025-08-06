import AgoraRTC, { AgoraRTCProvider } from "agora-rtc-react";
import { useEffect, useState } from "react";
import VideoCallPage from "./VideoCallPage";
import { Link } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import dateFormatter from "../../utils/dateFormatter";

export default function PreVideoCallPage() {
  const [appId] = useState("e9996d25de6b4cc3a63a796080af89f9");
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const [calling, setCalling] = useState(false);
  const [loadingToken, setLoadingToken] = useState(false);

  const client = AgoraRTC.createClient({ mode: "rtc", codec: "vp8" });

  const {
    data: videocallAppointmentData,
    error: videocallAppointmentError,
    isLoading: isVideocallAppoinmentLoading
  } = useQuery({
    queryKey: ['VideoCallAppointments'],
    queryFn: getAppointments
  });

  const handleJoinClick = async (appointmentId) => {
    setLoadingToken(true);
    try {
      const token = await getTokenFromBackend(appointmentId);
      if (token) {
        setSelectedAppointment(token);
        setCalling(true);
      } else {
        throw new Error("Failed to retrieve token.");
      }
    } 
    catch (error) 
    {
      console.error("Error fetching token:", error);
    } 
    finally 
    {
      setLoadingToken(false);
    }
  };


  if (isVideocallAppoinmentLoading) 
  {
    return <div>Loading...</div>;
  }

  if (videocallAppointmentError) 
  {
    return <div>{videocallAppointmentError}</div>;
  }
  

  if (calling && selectedAppointment) 
  {
     return (
       <AgoraRTCProvider client={client}>
         <VideoCallPage
           channelName={selectedAppointment.channelName}
           token={selectedAppointment.token}
           appId={appId}
           setCalling={setCalling}
         />
       </AgoraRTCProvider>
     );
  }

  
  if(!isVideocallAppoinmentLoading && videocallAppointmentData.length > 0)
  {
    return (
      <div className="p-6 bg-gray-100 max-h-full overflow-y-auto scrollbar-hide">
        {videocallAppointmentData?.map((app) => (
          <div key={app.appointmentId}
            className="bg-white h-full rounded-lg shadow-md p-6 mb-4 max-w-md mx-auto">

            <p className="text-lg font-semibold text-gray-800">Name: {app.counterpartName}</p>
            <p className="text-gray-600 mb-4">
              Schedule: {dateFormatter(app.appointmentSchedule)}
            </p>
            <button
              onClick={() => handleJoinClick(app.appointmentId)}
              className="px-4 py-2 bg-gray-700 text-white rounded hover:bg-blue-600 transition"
            >
              Join
            </button>
          </div>
        ))}
      </div>
    );
  }
  else {
    return (
      <div>
        <h1>Seems like you have no upcoming appointments!</h1>
        {localStorage.getItem('userRole') == 'patient' && (
          <>
            Book one&nbsp;          
            <Link to="/appointments" className="underline decoration-2 underline-offset-2">here</Link>
          </>
        )}
      </div>
    );
  }
}

const getTokenFromBackend = async (channelName) => 
{
  const completeUrl = `${localStorage.getItem("baseUrl")}/VideoCall/generate-agora-token?appointmentId=${channelName}`;
  
  try {
    const response = await fetch(completeUrl, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) 
    {
      throw new Error("Failed to fetch token");
    }

    const data = await response.json();
    return data;
  } 
  catch (error) 
  {
    console.error("Error fetching token:", error);
    return null;
  }
}

const getAppointments = async () => {
  const response = await fetch(`${localStorage.getItem('baseUrl')}/Appointments/VideoCallAppointments`, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      "Content-Type": "application/json",
    }
  });
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return response.json();
}