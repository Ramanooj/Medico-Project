import React, { useState, useEffect } from "react";

export default function PatientMedicalHistory({ patientId }) {
  const [medicalHistory, setMedicalHistory] = useState(null);

  useEffect(() => {
    if (patientId) {
      fetch(
        `${localStorage.getItem('baseUrl')}/MedicalHistory?patientId=${patientId}`,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
          },
        }
      )
        .then((response) => response.json())
        .then((data) => setMedicalHistory(data))
        .catch((error) =>
          console.error("Error fetching medical history:", error)
        );
    }
  }, [patientId]);

  if (!medicalHistory) {
    return <p>Loading medical history...</p>;
  }

  return (
    <div>
      <h2>Medical History for {medicalHistory.PatientName}</h2>
      <p>{medicalHistory.MedicalHistoryDescription}</p>
    </div>
  );
}

