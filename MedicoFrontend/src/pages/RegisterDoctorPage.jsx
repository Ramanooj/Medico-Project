import React, { useState } from 'react';
import '../styles/register/registerdoctorform.css';
import { useNavigate } from 'react-router-dom';

export default function RegisterDoctorFormPage() {
  const navigate = useNavigate()
  const [formData, setFormData] = useState({
    doctorCPSONum: '',
    clinicAddress: '',
    specialty: '',
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(`${localStorage.getItem('baseUrl')}/Doctors/Sign-up`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
          'X-ID-Token': `${localStorage.getItem('idToken')}`
        },
        body: JSON.stringify(formData),
      });

      if (response.ok) {

        navigate("/dashboard")
      } else {
        alert('Failed to register. Please try again.');
      }
    } catch (error) {
      console.error('Error during registration:', error);
      alert('An error occurred. Please try again later.');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="register-form">
      <h2>Doctor Registration</h2>
      <label>
        CPSO Number:
        <input type="text" name="doctorCPSONum" value={formData.doctorCPSONum} onChange={handleChange} required />
      </label>
      <label>
        Clinic Address:
        <input type="text" name="clinicAddress" value={formData.clinicAddress} onChange={handleChange} required />
      </label>
      <label>
        Specialty:
        <input type="text" name="specialty" value={formData.specialty} onChange={handleChange} required />
      </label>
      <button type="submit" className="submit-button">Submit</button>
    </form>
  );
}