import React, { useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";

export default function PrescriptionForm({ patientId }) {
  const queryClient = useQueryClient();
  const [prescriptionContent, setPrescriptionContent] = useState("");
  const [repeatNum, setRepeatNum] = useState(0);
  const [daysApart, setDaysApart] = useState(0);


  const {mutate: addNewPrescription} = useMutation({
    mutationFn: addPrescription,
    onSuccess: () => {
      queryClient.invalidateQueries({queryKey: ['getPatientPrescription']})
    }
  });

  return (
    <div className="bg-gray-100 flex items-center justify-center">
      <div className="w-full max-w-[500px] max-h-[670px] bg-white rounded-lg shadow-md p-6">
        <h2 className="text-2xl font-bold mb-4">Create Prescription</h2>
        <form
          onSubmit={(e) => {
            e.preventDefault();
            addNewPrescription({
              patientId,
              prescriptionContent,
              repeatNum,
              daysApart,
            });
          }}
        >
          <div className="mb-4">
            <label className="block text-gray-700 font-medium mb-2">
              Prescription Content:
            </label>
            <textarea
              className="w-full h-[300px] p-2 border border-gray-300 rounded-md resize-none focus:outline-none focus:border-blue-500"
              value={prescriptionContent}
              onChange={(e) => setPrescriptionContent(e.target.value)}
            />
          </div>

          <div className="mb-4">
            <label className="block text-gray-700 font-medium mb-2">
              Repeat Number:
            </label>
            <input
              type="number"
              className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:border-blue-500"
              value={repeatNum}
              onChange={(e) => setRepeatNum(Number(e.target.value))}
            />
          </div>

          <div className="mb-4">
            <label className="block text-gray-700 font-medium mb-2">
              Days Apart:
            </label>
            <input
              type="number"
              className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:border-blue-500"
              value={daysApart}
              onChange={(e) => setDaysApart(Number(e.target.value))}
            />
          </div>

          <button
            type="submit"
            className="w-full bg-gray-800 text-white py-2 rounded-md hover:bg-blue-600 transition"
          >
            Create Prescription
          </button>
        </form>
      </div>
    </div>
  );
}

const addPrescription = async (body) => {
  const baseUrl = `${localStorage.getItem('baseUrl')}/Prescriptions/Prescribe`;
  const response = await fetch(baseUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
    },
    body: JSON.stringify(body),
  });
  if (response.ok) {
    alert("Prescription created successfully");
  } else {
    const error = await response.json();
    alert(`Error: ${error?.Error || "Failed to create prescription"}`);
  }
}
