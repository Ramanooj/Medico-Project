import React, { useState, useEffect } from "react";
import { FaStethoscope, FaMapMarkerAlt, FaRoute} from 'react-icons/fa';

export default function DoctorList({ onSelectDoctor }) {
  const [doctors, setDoctors] = useState([]);

  useEffect(() => {
    const baseUrl = `${localStorage.getItem('baseUrl')}/Appointments/Proximity`;
    const params = new URLSearchParams({
      patientAddress: "",
      distanceInKM: 14,
    });
    const urlWithParams = `${baseUrl}?${params.toString()}`;
    
    fetch(urlWithParams, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
    })
      .then((response) => response.json())
      .then((data) => setDoctors(data))
      .catch((error) => console.error("Error fetching doctors:", error));
  }, []);

  return (
    <div>
      <h2 className="text-2xl font-bold mb-4">Select a Doctor</h2>
      <ul className="space-y-4">
        {doctors.map((doctor) => (
          <li
            key={doctor.doctorId}
            onClick={() => onSelectDoctor({doctorId: doctor.doctorId, doctorName: doctor.doctorName})}
            className="p-4 bg-white border rounded-lg shadow-md cursor-pointer hover:bg-gray-100 transition-colors"
          >
            <p className="font-semibold text-lg">
              {doctor.doctorName}
            
            </p>
            <p className="font-semibold text-lg flex items-center">
              <FaStethoscope className="mr-2 text-gray-400" />
              {doctor.specialty}
            </p>
            <p className="font-semibold text-lg flex items-center">
            <FaMapMarkerAlt className="mr-2 text-gray-400" />
              {doctor.clinicAddress}</p>
            <p className="font-semibold text-lg flex items-center">
              <FaRoute className="mr-2 text-gray-400"/>
              {doctor.estimatedDistance}
            </p>
          </li>
        ))}
      </ul>
    </div>
  );
}
