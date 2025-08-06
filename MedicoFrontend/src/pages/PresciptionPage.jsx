import React, { useState } from "react";
import PatientList from "../components/reusable/PatientList";
import PrescriptionList from "../components/prescription/PrescriptionList";
import PrescriptionForm from "../components/prescription/PrescriptionForm";
import { FaArrowLeft } from 'react-icons/fa';

function PrescriptionPage() {
  const [selectedPatient, setSelectedPatient] = useState(null);

  return (
    <div className=" bg-gray-100">
      {!selectedPatient && (
        <PatientList setSelectedPatient={setSelectedPatient} />
      )}

      {selectedPatient && (
        <>
            <button className="flex mb-5 items-center px-5 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition" onClick={() => setSelectedPatient(null)}>
              <FaArrowLeft className="mr-2 text-lg" /> Back
            </button>

          <div className="flex flex-col md:flex-row">

            <div className="w-full">
              <PrescriptionForm patientId={selectedPatient} />
            </div>

            <div className="w-full">
              <PrescriptionList patientId={selectedPatient} />
            </div>
          </div>
        </>
      )}
    </div>
  );
}

export default PrescriptionPage;
