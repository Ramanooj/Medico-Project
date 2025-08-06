import React, { useState, useEffect } from "react";

export default function PatientList({ setSelectedPatient }) {
  const [patients, setPatients] = useState([]);

  useEffect(() => {
    const baseUrl = `${localStorage.getItem('baseUrl')}/Prescriptions/Patients`;
    fetch(baseUrl, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
    })
      .then((response) => response.json())
      .then((data) => setPatients(data))
      .catch((error) => console.error("Error fetching patients:", error));
  }, []);

  return (
    <div className="bg-gray-100">
      <h2 className="text-2xl font-bold mb-4">Select a Patient</h2>
      <ul className="space-y-3">
        {patients.map((patient) => (
          <li
            key={patient.patientId}
            className="p-4 bg-white rounded-lg shadow hover:bg-blue-50 cursor-pointer transition-colors"
            onClick={() => setSelectedPatient(patient.patientId)}
          >
            <p className="text-lg font-medium text-gray-900">{patient.patientFullName}</p>
            <p className="text-sm text-gray-500">{patient.patientId}</p>
          </li>
        ))}
      </ul>
    </div>
  );
}
