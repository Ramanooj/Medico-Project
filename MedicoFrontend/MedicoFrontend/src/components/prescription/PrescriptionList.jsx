import React, { useEffect } from "react";
import dateFormatter from "../../utils/dateFormatter";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";


const deletePrescription = async (prescriptionId) => {
  try {
    const response = await fetch(
      `${localStorage.getItem('baseUrl')}/Prescriptions/Delete?prescriptionId=${prescriptionId}`,
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
        },
      }
    );

    if (!response.ok) {
      alert("Failed to delete prescription");
    }
    
  } catch (error) {
    console.error("Error deleting prescription:", error);
    alert("An error occurred while deleting the prescription.");
  }
};

async function getPrescription(patientId) {
  const response = await fetch(
    `${localStorage.getItem('baseUrl')}/Prescriptions/Prescriptions?patientId=${patientId}`,
    {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
      },
    })

  if (!response.ok) {
    throw new Error('Network response was not ok');
  }

  return response.json();
}

function PrescriptionList({ patientId }) 
{
  const queryClient = useQueryClient();
  
  const {
    data: prescriptionData,
    erorr: prescriptionError,
    isLoading: isPrescriptionLoading

  } = useQuery({
    queryKey: ['getPatientPrescription'],
    queryFn: () => getPrescription(patientId)
  });

  const {mutate: deletePatientPrescription} = useMutation({
    mutationFn: deletePrescription,
    onSuccess: () => {
      queryClient.invalidateQueries( {queryKey:  ['getPatientPrescription'] })
    },
  });

  if(isPrescriptionLoading)
  {
    return <h1>Loading.....</h1>;
  }
  if(!isPrescriptionLoading && prescriptionError)
  {
    return <h1>something went wrong!</h1>
  }

  return (
    <div className="bg-gray-100 rounded-lg shadow-md">
      <div className="overflow-x-auto max-h-[670px] overflow-y-auto">
        <table className="min-w-fit bg-white rounded-lg shadow-md">
          <thead className="bg-gray-800 text-white uppercase text-sm sticky top-0">
            <tr>
              <th className="py-3 px-6 text-left">Patient Name</th>
              <th className="py-3 px-6 text-left">Prescription Content</th>
              <th className="py-3 px-6 text-left">Repeat Number</th>
              <th className="py-3 px-6 text-left">Days Apart</th>
              <th className="py-3 px-6 text-left">Issue Date / Time</th>
              <th className="py-3 px-6 text-left">Actions</th>
            </tr>
          </thead>
          <tbody>
            {!isPrescriptionLoading && prescriptionData && prescriptionData.map((prescription) => (
              <tr key={prescription.prescriptionId} className="border-b hover:bg-gray-100">
                <td className="py-3 px-6 text-gray-700">{prescription.patientName}</td>
                <td className="py-3 px-6 text-gray-700">{prescription.prescriptionContent}</td>
                <td className="py-3 px-6 text-gray-700">{prescription.repeatNum}</td>
                <td className="py-3 px-6 text-gray-700">{prescription.daysApart}</td>
                <td className="py-3 px-6 text-gray-700">{dateFormatter(prescription.issueDate)}</td>
                <td className="py-3 px-6">
                  <button
                    className="text-red-500 hover:underline"
                    onClick={() => deletePatientPrescription(prescription.prescriptionId)}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default PrescriptionList;