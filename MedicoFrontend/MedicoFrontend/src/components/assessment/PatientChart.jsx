import React, { useState, useEffect } from "react";
import { useQuery } from "@tanstack/react-query";

function PatientChart({ patientId }) {
  
  const {
    data: patientChartData,
    error: patientChartError,
    isLoading: isPatientChartLoading
  } = useQuery({
    queryKey: ['getPatientChart'],
    queryFn: () => getPatientChart(patientId),
  })

  if(isPatientChartLoading) {
    return <p>Loading patient chart...</p>;
  }
  if(patientChartError)
  {
    return <h1>Something went wrong!</h1>
  }

  if(!isPatientChartLoading && patientChartData)
  {
    return (
      <div className="bg-gray-100 flex items-center justify-center">
        <div className="w-full max-w-2xl bg-white rounded-lg shadow-md p-6 h-[670px] overflow-y-auto">
          <h1 className="text-2xl font-bold mb-4">Chart</h1>
  
          {/* Patient Information Card */}
          <div className="mb-6">
            <h3 className="text-xl font-semibold mb-2">Patient Information</h3>
            <p className="text-gray-700">
              <strong>Name:</strong> {patientChartData.patient?.firstName} {patientChartData.patient?.lastName}
            </p>
            <p className="text-gray-700">
              <strong>Phone Number:</strong> {patientChartData.patient?.phoneNumber}
            </p>
            <p className="text-gray-700">
              <strong>Date of Birth:</strong> {patientChartData.patient?.dateOfBirth}
            </p>
            <p className="text-gray-700">
              <strong>Health Card Number:</strong> {patientChartData.patient?.healthCardNumber}
            </p>
            <p className="text-gray-700">
              <strong>Gender:</strong> {patientChartData.patient?.gender}
            </p>
            <p className="text-gray-700">
              <strong>Address:</strong> {patientChartData.patient?.patientAddress}
            </p>
          </div>
  
          {/* Medical History */}
          <div className="mb-6">
            <h3 className="text-xl font-semibold mb-2">Medical History</h3>
            <p className="text-gray-700">{patientChartData.medicalHistoryDescription}</p>
          </div>
  
          {/* Assessment */}
          <div className="mb-6">
            <h3 className="text-xl font-semibold mb-2">Assessment</h3>
            <p className="text-gray-700">{patientChartData.assessmentDescription}</p>
          </div>
  
          {/* Allergies */}
          <div className="mb-6">
            <h3 className="text-xl font-semibold mb-2">Allergies</h3>
            <ul className="list-disc list-inside text-gray-700">
              {patientChartData.patient?.allergies.map((allergy, index) => (
                <li key={index}>{allergy.allergyName}</li>
              ))}
            </ul>
          </div>
  
          {/* Medications */}
          <div>
            <h3 className="text-xl font-semibold mb-2">Medications</h3>
            <ul className="list-disc list-inside text-gray-700">
              {patientChartData.patient?.medications.map((med, index) => (
                <li key={index}>{med.medicationDescription}</li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    );
  }
  else {
    return <p>Something went wrong</p>;
  }
}


async function getPatientChart(patientId) {
  const baseUrl = `${localStorage.getItem('baseUrl')}/Assessments/Chart?patientId=`;

  const response = await fetch(`${baseUrl}${patientId}`, {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("accessToken")}`
    }
  })
  
  if(!response.ok)
  {
    throw new Error('Network response was not ok');
  }
  
  return response.json();
    
}


export default PatientChart;