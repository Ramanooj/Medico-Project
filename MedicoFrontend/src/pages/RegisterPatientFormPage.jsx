import React, { useState } from 'react';
import '../styles/register/registerpatientform.css';
import { useNavigate } from 'react-router-dom';

export default function RegisterPatientFormPage() {
  const navigate = useNavigate()
  const [formData, setFormData] = useState({
    gender: '',
    dateOfBirth: '',
    healthCardNumber: '',
    ptFullAddress: '',
    allergies: [{ AllergyName: '' }],
    medications: [{ MedicationDescription: '' }],
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleAllergiesChange = (index, value) => {
    const updatedAllergies = [...formData.allergies];
    updatedAllergies[index].AllergyName = value;
    setFormData({ ...formData, allergies: updatedAllergies });
  };

  const handleMedicationsChange = (index, value) => {
    const updatedMedications = [...formData.medications];
    updatedMedications[index].MedicationDescription = value;
    setFormData({ ...formData, medications: updatedMedications });
  };

  const addAllergyField = () => {
    setFormData({
      ...formData,
      allergies: [...formData.allergies, { AllergyName: '' }],
    });
  };

  const addMedicationField = () => {
    setFormData({
      ...formData,
      medications: [...formData.medications, { MedicationDescription: '' }],
    });
  };

  const removeAllergyField = (index) => {
    const updatedAllergies = formData.allergies.filter((_, i) => i !== index);
    setFormData({ ...formData, allergies: updatedAllergies });
  };

  const removeMedicationField = (index) => {
    const updatedMedications = formData.medications.filter((_, i) => i !== index);
    setFormData({ ...formData, medications: updatedMedications });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch(`${localStorage.getItem('baseUrl')}/Patients/Sign-up`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
          'X-ID-Token': `${localStorage.getItem('idToken')}`,
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
      <h2>Patient Registration</h2>
      <label>Gender:</label>
      <div>
        <label>
          <input
            type="radio"
            name="gender"
            value="M"
            checked={formData.gender === "M"}
            onChange={handleChange}
            required
          />
          Male
        </label>
        <label>
          <input
            type="radio"
            name="gender"
            value="F"
            checked={formData.gender === "F"}
            onChange={handleChange}
            required
          />
          Female
        </label>
      </div>  
      <label>
        Date of Birth:
        <input type="date" name="dateOfBirth" value={formData.dateOfBirth} onChange={handleChange} required />
      </label>
      <label>
        Health Card Number:
        <input type="text" name="healthCardNumber" value={formData.healthCardNumber} onChange={handleChange} required />
      </label>
      <label>
        Full Address:
        <input type="text" name="ptFullAddress" value={formData.ptFullAddress} onChange={handleChange} required />
      </label>

      <div className="dynamic-inputs">
        <label>Allergies (optional):</label>
        {formData.allergies.map((allergy, index) => (
          <div key={index} style={{ display: 'flex', alignItems: 'center' }}>
            <input
              type="text"
              placeholder="Allergy Name"
              value={allergy.AllergyName}
              onChange={(e) => handleAllergiesChange(index, e.target.value)}
            />
            <button type="button" onClick={() => removeAllergyField(index)}>Remove</button>
          </div>
        ))}
        <button type="button" onClick={addAllergyField}>Add Allergy</button>
      </div>

      <div className="dynamic-inputs">
        <label>Medications (optional):</label>
        {formData.medications.map((medication, index) => (
          <div key={index} style={{ display: 'flex', alignItems: 'center' }}>
            <input
              type="text"
              placeholder="Medication Description"
              value={medication.MedicationDescription}
              onChange={(e) => handleMedicationsChange(index, e.target.value)}
            />
            <button type="button" onClick={() => removeMedicationField(index)}>Remove</button>
          </div>
        ))}
        <button type="button" onClick={addMedicationField}>Add Medication</button>
      </div>

      <button type="submit" className="submit-button">Submit</button>
    </form>
  );
}
