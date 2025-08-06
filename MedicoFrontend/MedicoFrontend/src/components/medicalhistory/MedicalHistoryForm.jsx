import { useState, useEffect } from "react";

export default function MedicalHistoryForm( { patientId } )
{ 
   const [medicalHistory, setMedicalHistory] = useState({
     patientId: "",
     patientName: "",
     medicalHistoryDescription: "",
   });

     useEffect(() => {
     if (patientId) {
       fetch(
         `${localStorage.getItem('baseUrl')}/Assessments/MedicalHistory?patientId=${patientId}`,
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
           console.error("Error fetching prescriptions:", error)
         );
     }
   }, [patientId]);

   const handleSubmit = async (e) => {
    e.preventDefault();

     try {
       const response = await fetch(
         `${localStorage.getItem('baseUrl')}/Assessments/MedicalHistory`,
         {
           method: "POST",
           headers: {
             "Content-Type": "application/json",
             Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
           },
           body: JSON.stringify({
             patientId: `${patientId}`,
             AssessmentDescription: `${medicalHistory.medicalHistoryDescription}`,
           }),
         }
       );

       if (response.ok) {
         alert("Added Medical History successfully");
       } else {
         alert("Failed to add new medical history");
       }
     } catch (error) {
       console.error("Error adding new medical histry:", error);
       alert("An error occurred while adding the medical histry:");
     }
   };


   return (
    <div className="p-6 bg-gray-100 flex items-center justify-center">
      <div className="w-[672px] bg-white rounded-lg shadow-md h-[670px] p-6">
        <h2 className="text-2xl font-semibold mb-4">
          Patient Name:&nbsp;<span className="font-bold underline underline-offset-4 decoration-2">{medicalHistory.patientName}</span>
        </h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-gray-700 font-medium mb-2">
              Description:
            </label>
            <textarea
              className="w-full h-texAreaSize p-2 border border-gray-300 rounded-md resize-none focus:outline-none focus:border-blue-500"
              value={medicalHistory.medicalHistoryDescription}
              onChange={(e) =>
                setMedicalHistory((currentVal) => ({
                  ...currentVal,
                  medicalHistoryDescription: e.target.value,
                }))
              }
            />
          </div>
          <button type="submit" className="w-full bg-gray-800 text-white py-2 rounded-md hover:bg-blue-600 transition">
            Update Medical History
          </button>
        </form>
      </div>
    </div>
  );
}

