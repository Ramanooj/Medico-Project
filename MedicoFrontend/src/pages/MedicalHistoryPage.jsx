import React, { useState } from "react";
import PatientList from "../components/reusable/PatientList";
import MedicalHistoryForm from "../components/medicalhistory/MedicalHistoryForm";
import { FaArrowLeft } from 'react-icons/fa';


export default function MedicalHistoryPage()
{
    const [selectedPatient, setSelectedPatient] = useState(null);

    return (
      <div>
        {!selectedPatient && (
          <PatientList setSelectedPatient={setSelectedPatient} />
        )}

        {selectedPatient && (
          <>
          <button className="flex mb-5 items-center px-5 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition" onClick={() => setSelectedPatient(null)}>
          <FaArrowLeft className="mr-2 text-lg" /> Back
          </button>
          
          <MedicalHistoryForm patientId={selectedPatient}/>
         </>  
        )}
      </div>
    );
}