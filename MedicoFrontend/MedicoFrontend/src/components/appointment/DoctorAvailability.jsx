import React, { useState, useEffect } from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import AOS from 'aos';
import { FaArrowLeft } from 'react-icons/fa';
import 'aos/dist/aos.css';
import { parseISO, format } from "date-fns";

export default function DoctorAvailability({doctorId, onSelectTimeslot, setSelectedDoctor,}) 
{
  const [availability, setAvailability] = useState({});
  const [selectedDate, setSelectedDate] = useState(null);

  useEffect(() => {
    const baseUrl = `${localStorage.getItem("baseUrl")}/Appointments/Availability`;
    AOS.init({ duration: 1000});
    if (doctorId) 
    {
      fetch(`${baseUrl}?doctorId=${doctorId}`, {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
        },
      })
        .then((response) => response.json())
        .then((data) => {
          setAvailability(data);
          const initialDateKey = Object.keys(data)[0];
          if (initialDateKey) {
            setSelectedDate(parseISO(initialDateKey));
          }
        })
        .catch((error) => console.error("Error fetching availability:", error));
    }
  }, [doctorId]);

  function Back() 
  {
    setSelectedDoctor(null);
  }

  if (!selectedDate) 
    {
    return <p>Loading...</p>;
  }

  const availableDates = Object.keys(availability);
  const selectedDateString = format(selectedDate, "yyyy-MM-dd");
  const timeslots = availability[selectedDateString] || [];

  const tileClassName = ({ date, view }) => {
    if (view === "month") {
      const dateString = format(date, "yyyy-MM-dd");
      if (availableDates.includes(dateString)) {
        return "highlight";
      }
    }
    return null;
  };

  return (
    <div className="p-6 bg-gray-100">
      <div className="bg-white rounded-lg shadow-lg p-8 max-w-4xl mx-auto">

      <button onClick={Back} className="flex items-center px-5 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition">
        
        <FaArrowLeft className="mr-2 text-lg" /> Back
      </button>

      <h2 className="text-2xl font-bold mb-4 text-center">Available Timeslots</h2>
      <div className="flex justify-center mb-6">
        <Calendar
          onChange={(date) => setSelectedDate(date)}
          value={selectedDate}
          tileClassName={tileClassName}
          tileDisabled={({ date, view }) => {
            if (view === "month") {
              const dateString = format(date, "yyyy-MM-dd");
              return !availableDates.includes(dateString);
            }
            return false;
          }}
          minDate={new Date(new Date().getFullYear(), new Date().getMonth(), 1)}
          maxDate={new Date(new Date().getFullYear(), new Date().getMonth() + 2, 0)}
          className="rounded-lg shadow-md"
        />
      </div>
        <h3 className="text-xl font-semibold mb-4">
          Available Times for&nbsp;
          <span className="inline underline decoration-1 underline-offset-2">{selectedDate.toDateString()}</span>
        </h3>
        {timeslots.length > 0 ? (
          <ul data-aos="fade-left" className=" grid grid-cols-2 gap-4 md:grid-cols-3 ">
            {timeslots.map((time, index) => (
              <li
                key={index}
                onClick={() => onSelectTimeslot(format(selectedDate, "yyyy-MM-dd"), time)}
                className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 cursor-pointer transition"
              >
                {time}
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-500">No available times for this date.</p>
        )}
      </div>
    </div>
  );
}
