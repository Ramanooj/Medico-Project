import React, { useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { FaArrowLeft } from 'react-icons/fa';
import { format } from "date-fns";
import { useNavigate } from 'react-router-dom';

export default function AppointmentForm({ doctorName, doctorId, date, time, setSelectedTimeslot }) 
{
  const queryClient = useQueryClient();
  const [reason, setReason] = useState("");
  const [isToBeNotified, setIsToBeNotified] = useState(false);
  const formattedDate = format(date, "yyyy-MM-dd")
  const navigate = useNavigate();


  const {mutate: createNewAppointment} = useMutation({
     mutationFn: (body) => createAppointment(body, navigate),
     onSuccess: () => {
      queryClient.invalidateQueries({queryKey: ["VideoCallAppointments"]});
      queryClient.invalidateQueries({queryKey: ["patientDashboard"]});
     }
  })

  function Back() {
    setSelectedTimeslot({
      date: "",
      time: "",
    });
  }

  return (
    <div className="p-6 bg-gray-100 flex justify-center items-center">
      <div className="bg-white rounded-lg shadow-md p-8 w-full max-w-lg">
      <button
          onClick={Back}
          className="flex items-center px-5 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition"
        >
           <FaArrowLeft className="mr-2 text-lg" /> Back
        </button>
        <form
          onSubmit={(e) => {
            e.preventDefault();
            createNewAppointment({
              doctorId,
              appmntDate: date,
              appmntTime: time,
              reason,
              isToBeNotified,
            });
          }}
          className="space-y-4"
        >
          <h2 className="text-2xl font-bold mb-4">Create Appointment</h2>
          <p className="text-gray-700">Doctor Name: <span className="font-medium">{doctorName}</span></p>
          <p className="text-gray-700">Date: <span className="font-medium">{formattedDate}</span></p>
          <p className="text-gray-700">Time: <span className="font-medium">{time}</span></p>
  
          <label className="block text-gray-700">
            Reason:
            <input
              type="text"
              value={reason}
              onChange={(e) => setReason(e.target.value)}
              className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 p-2"
            />
          </label>
  
          <label className="flex items-center text-gray-700">
            <input
              type="checkbox"
              checked={isToBeNotified}
              onChange={(e) => setIsToBeNotified(e.target.checked)}
              className="mr-2"
            />
            Notify Me (30 mins before)
          </label>
  
          <button
            type="submit"
            className="w-full py-2 bg-gray-800 text-white rounded hover:bg-gray-600 transition"
          >
            Create Appointment
          </button>
        </form>
      </div>
    </div>
  );
}


const createAppointment = async (body, navigate) => {
  console.log(navigate)
  const completeUrl = `${localStorage.getItem('baseUrl')}/Appointments/Create`;
  try {
    const response = await fetch(completeUrl, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    });

    if (response.ok) {
      navigate('/dashboard');
    } 
    else {
      const error = await response.json();
      alert(`Error: ${error?.Error || "Failed to create appointment"}`);
    }
  } catch (error) {
    console.error("Error creating appointment:", error);
    alert("An error occurred while creating the appointment.");
  }
};