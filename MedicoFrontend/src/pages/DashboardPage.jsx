import React, { useEffect, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import dateFormatter from '../utils/dateFormatter';
import AOS from 'aos';
import 'aos/dist/aos.css';

const DashboardPage = () => {
  const [role, setRole] = useState(null);


  useEffect(() => {
    const storedRole = localStorage.getItem('userRole');
    if (storedRole) {
      setRole(storedRole);
    }
    AOS.init({ duration: 1000});
  }, []);

  const {
    data: doctorData,
    error: doctorError,
    isLoading: isLoadingDoctor
  } = useQuery({
    queryKey: ['doctorDashboard'],
    queryFn: fetchDoctorDashboard,
    enabled: role === 'doctor',
  });

  const {
    data: patientData,
    error: patientError,
    isLoading: isLoadingPatient
  } = useQuery({
    queryKey: ['patientDashboard'],
    queryFn: fetchPatientDashboard,
    enabled: role === 'patient',
  });

  if (!role) return <div>Loading role...</div>;

  if (role === 'doctor') 
  {
    if (isLoadingDoctor) 
    {
      return <div>Loading Doctor Dashboard...</div>;
    }
    if (doctorError) 
    {
      return <div>Error: {doctorError.message}</div>;
    }

    return (
    <div>
    <h3 className="text-xl font-bold">Appointments</h3>
    <table className="w-full border-collapse mt-4">
        <thead>
            <tr className="bg-gray-800 text-white">
                <th className="border p-2">Date</th>
                <th className="border p-2">Time</th>
                <th className="border p-2">Patient</th>
                <th className="border p-2">Reason</th>
            </tr>
        </thead>
        <tbody>
        {doctorData &&
          doctorData.map((appointment) => (
            <tr key={appointment.appointmentId}>
              <td className="border p-2">{appointment.appmntDate}</td>
              <td className="border p-2">{appointment.appmntTime}</td>
              <td className="border p-2">{appointment.patientFullName}</td>
              <td className="border p-2">{appointment.reason}</td>
            </tr>
          ))}
        </tbody>
    </table>
    </div>
    );
  }

  else if (role === 'patient') 
  {
    if (isLoadingPatient) 
    {
      return <div>Loading Patient Dashboard...</div>;
    }
    if (patientError)
    {
      return <div>Error: {patientError.message}</div>;
    }

    return (
      <div>
        <h3 className="text-xl font-bold">Upcoming Appointments</h3>
        <table className="w-full border-collapse mt-4">
            <thead>
                <tr className="bg-gray-800 text-white">
                    <th className="border p-2">Date / Time</th>
                    <th className="border p-2">Doctor</th>
                    <th className="border p-2">Reason</th>
                </tr>
            </thead>
            <tbody>
                    {patientData?.appointments?.map((appointment) => {
                    const formatDate = dateFormatter(appointment.appointmentSchedule);
                    return (
                      <tr key={appointment.appointmentId}>
                      <td>{formatDate}</td>
                      <td>{appointment.doctorFullName}</td>
                      <td>{appointment.reason}</td>
                    </tr>
                    );
                  })}
          </tbody>
        </table>


        <div className="mt-8">
        <h3 className="text-xl font-bold">Prescription</h3>
          <table className="w-full border-collapse mt-4">
            <thead>
                <tr className="bg-gray-800 text-white">
                    <th className="border p-2">Content</th>
                    <th className="border p-2">Issue Date</th>
                    <th className="border p-2">Doctor</th>
                </tr>
            </thead>
            <tbody>
                  {patientData?.prescriptions?.map((prescription) => (
                    <tr key={prescription.prescriptionId}>
                      <td>{prescription.prescriptionContent}</td>
                      <td>{dateFormatter(prescription.issueDate)}</td>
                      <td>{prescription.doctorName}</td>
                    </tr>
                  ))}
            </tbody>
        </table>
        
        </div>
      </div>
    );
  } else {
    return <div>No role specified</div>;
  }
};


const fetchDoctorDashboard = async () => {
  const response = await fetch(`${localStorage.getItem('baseUrl')}/Doctors/Dashboard`, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      "Content-Type": "application/json",
    }
  });
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return response.json();
};

const fetchPatientDashboard = async () => {
  const response = await fetch(`${localStorage.getItem('baseUrl')}/Patients/Dashboard`, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      "Content-Type": "application/json",
    }
  });
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return response.json();
};


export default DashboardPage;
