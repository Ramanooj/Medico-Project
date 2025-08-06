import React, { useState } from "react";
import PatientList from "../components/reusable/PatientList";
import AssessmentForm from "../components/assessment/AssessmentForm";
import PatientChart from "../components/assessment/PatientChart";
import { FaArrowLeft } from 'react-icons/fa';

export default function AssessmentPage() {
  const [selectedPatient, setSelectedPatient] = useState(null);

  return (
    <div className="bg-gray-100">
      {!selectedPatient && (
        <PatientList setSelectedPatient={setSelectedPatient} />
      )}

      {selectedPatient && (
        <>
          <button className="flex mb-5 items-center px-5 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition" onClick={() => setSelectedPatient(null)}>
          <FaArrowLeft className="mr-2 text-lg" /> Back
          </button>

          <div className="flex flex-col md:flex-row space-y-6 md:space-y-0 md:space-x-6">
            <div className="flex-1">
              <AssessmentForm patientId={selectedPatient} />
            </div>
            <div className="flex-1">
              <PatientChart patientId={selectedPatient} />
            </div>
          </div>
        </>
      )}
    </div>
  );
}
