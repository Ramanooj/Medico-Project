import React, { useState } from "react";
import DoctorList from "../components/appointment/DoctorList";
import DoctorAvailability from "../components/appointment/DoctorAvailability";
import AppointmentForm from "../Components/Appointment/AppointmentForm";

export default function AppointmentPage() {
  const [selectedDoctor, setSelectedDoctor] = useState(null);
  const [selectedTimeslot, setSelectedTimeslot] = useState({
    date: "",
    time: "",
  });

  return (
    <div>
      {!selectedDoctor && <DoctorList onSelectDoctor={setSelectedDoctor} />}

      {selectedDoctor && !selectedTimeslot.date && (
        <DoctorAvailability
          doctorId={selectedDoctor.doctorId}
          onSelectTimeslot={(date, time) => setSelectedTimeslot({ date, time })}
          setSelectedDoctor={setSelectedDoctor}
        />
      )}

      {selectedTimeslot.date && (
        <AppointmentForm
          doctorName={selectedDoctor.doctorName}
          doctorId={selectedDoctor.doctorId}
          date={selectedTimeslot.date}
          time={selectedTimeslot.time}
          setSelectedTimeslot={setSelectedTimeslot}
        />
      )}
    </div>
  );
}
