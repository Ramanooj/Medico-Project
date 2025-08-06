import React, { useEffect, useState } from 'react';

export default function ProfilePage({ userRole }) {
  const [profileData, setProfileData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  let urlController = ""
  if(userRole)
  {
    if(userRole === 'patient')
    {
        urlController = "Patients"
    }
    if(userRole === 'doctor')
    {
        urlController = "Doctors"
    }
  }

  useEffect(() => {
    const fetchProfileData = async () => {
      try {
        setLoading(true);
        const response = await fetch(`${localStorage.getItem('baseUrl')}/${urlController}/profile`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
          },
        });

        if (!response.ok) {
          throw new Error('Failed to fetch profile data');
        }

        const data = await response.json();
        setProfileData(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchProfileData();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!profileData) {
    return <div>No profile data available.</div>;
  }

  return (
    <div className="flex justify-center bg-gray-100">
      <div className="p-10 bg-white rounded-lg shadow-lg w-full max-w-3xl">
        {userRole === 'patient' ? (
          <PatientProfile profile={profileData} />
        ) : (
          <DoctorProfile profile={profileData} />
        )}
      </div>
    </div>
  );
}

function PatientProfile({ profile }) {
  return (
    <div className="mb-10">
      <h2 className="text-black font-bold text-3xl mb-4">
        {profile.firstName} {profile.lastName}
      </h2>
      <p className="text-lg my-2"><strong>Email:</strong> {profile.email}</p>
      <p className="text-lg my-2"><strong>Phone Number:</strong> {profile.phoneNumber}</p>
      <p className="text-lg my-2"><strong>Date of Birth:</strong> {profile.dateOfBirth}</p>
      <p className="text-lg my-2"><strong>Health Card Number:</strong> {profile.healthCardNumber}</p>
      <p className="text-lg my-2"><strong>Address:</strong> {profile.patientAddress}</p>
      <p className="text-lg my-2"><strong>Gender:</strong> {profile.gender}</p>

      <h3 className="font-semibold mt-4">Allergies</h3>
      <ul className="list-disc list-inside">
        {profile.allergies.map((allergy) => (
          <li key={allergy.allergyId}>{allergy.allergyName}</li>
        ))}
      </ul>

      <h3 className="font-semibold mt-4">Medications</h3>
      <ul className="list-disc list-inside">
        {profile.medications.map((medication) => (
          <li key={medication.medicationId}>{medication.medicationDescription}</li>
        ))}
      </ul>
    </div>
  );
}

function DoctorProfile({ profile }) {
  return (
    <div className="mb-10">
      <h2 className="text-black font-bold text-3xl mb-4">
        Dr. {profile.firstName} {profile.lastName}
      </h2>
      <p className="text-lg my-2"><strong>Email:</strong> {profile.email}</p>
      <p className="text-lg my-2"><strong>Phone Number:</strong> {profile.doctorPhone}</p>
      <p className="text-lg my-2"><strong>License Number (CPSO):</strong> {profile.doctorCPSONum}</p>
      <p className="text-lg my-2"><strong>Specialty:</strong> {profile.specialty}</p>
      <p className="text-lg my-2"><strong>Clinic Address:</strong> {profile.clinicAddress}</p>
    </div>
  );
}
