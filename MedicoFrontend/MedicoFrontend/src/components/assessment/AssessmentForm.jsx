import React, { useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";

export default function AssessmentForm({ patientId }) {
  const [assessmentDescription, setAssessmentDescription] = useState("");
  const queryClient = useQueryClient();

  const {mutate: addNewAssessment } = useMutation({
    mutationFn: addAssessment,
    onSuccess: () => {
    queryClient.invalidateQueries({queryKey: ['getPatientChart']});
    }
  });
  
  return (
    <div className="bg-gray-100 flex items-center justify-center">
      <div className="w-[672px] bg-white rounded-lg shadow-md h-[670px] p-6">
        <h2 className="text-2xl font-bold mb-4">Create Assessment</h2>
        <form
          onSubmit={(e) => {
            e.preventDefault();
            addNewAssessment({
              patientId,
              assessmentDescription
            });
          }}
        >
          <div className="mb-4">
            <label className="block text-gray-700 font-medium mb-2">
              Assessment Description:
            </label>
            <textarea
              className="w-full h-texAreaSize p-2 border resize-none border-gray-300 rounded-md focus:outline-none focus:border-blue-500"
              value={assessmentDescription}
              onChange={(e) => setAssessmentDescription(e.target.value)}
            />
          </div>

          <button type="submit" className="w-full bg-gray-800 text-white py-2 rounded-md hover:bg-blue-600 transition">
            Create Assessment
          </button>
        </form>
      </div>
    </div>
  );
}

const addAssessment = async (body) => {
  const response = await fetch(
    `${localStorage.getItem('baseUrl')}/Assessments/Assessment`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
      body: JSON.stringify(body),
    }
  );
  if (!response.ok) {
    const error = await response.json();
    alert(`Error: ${error?.Error || "Failed to create assessment"}`);
  } 
};

