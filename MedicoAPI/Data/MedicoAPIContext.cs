using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicoAPI.Models;
using System.Configuration;
using System.Transactions;

namespace MedicoAPI.Data
{
    public class MedicoAPIContext : DbContext
    {
        public MedicoAPIContext (DbContextOptions<MedicoAPIContext> options)
            : base(options)
        {
            //DISBALED LAZY LOADING TO AVOID CIRCULAR QUERY ON THE DATABASE
            this.ChangeTracker.LazyLoadingEnabled = false;
        }


        public DbSet<Doctor> Doctor { get; set; } = default!;
        public DbSet<Appointment> Appointment { get; set; } = default!;
        public DbSet<Prescription> Prescription { get; set; } = default!;
        public DbSet<Patient> Patient { get; set; } = default!;
        public DbSet<Allergy> Allergies { get; set; } = default!;
        public DbSet<Medication> Medications { get; set; } = default!;
        public DbSet<MedicalHistory> MedicalHistories { get; set; } = default!;
        public DbSet<PatientAssessment> Assessments { get; set; } = default!;





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //SETTING UP PRIMARY KETS
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.PatientId);
            modelBuilder.Entity<Doctor>()
                .HasKey(d => d.DoctorId);
            modelBuilder.Entity<Allergy>()
                .HasKey(a => a.AllergyId);
            modelBuilder.Entity<MedicalHistory>()
                .HasKey(mh => mh.MedicalHistoryId);
            modelBuilder.Entity<Appointment>()
                .HasKey(app => app.AppointmentId);
            modelBuilder.Entity<Prescription>()
                .HasKey(pr => pr.PrescriptionId);
            modelBuilder.Entity<Medication>()
                .HasKey(md => md.MedicationId);
            modelBuilder.Entity<PatientAssessment>()
                .HasKey(ass => ass.AssessmentId);



            /* FOREIGN KEY / RELATIONSHIPS DEFINITION */

            //PATIENT - APPOINTMENT
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            //PATIENT - ASSESSMENT
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Assessments)
                .WithOne(ass => ass.Patient)
                .HasForeignKey(ass => ass.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            //PATIENT - PRESCRIPTION
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Prescriptions)
                .WithOne(pr => pr.Patient)
                .HasForeignKey(pr => pr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            //PATIENT - MEDICATION
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Medications)
                .WithOne(m => m.Patient)
                .HasForeignKey(med => med.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            //PATIENT - ALLERGIES
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Allergies)
                .WithOne(a => a.Patient)
                .HasForeignKey(alg => alg.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            //==========================================

            //DOCTOR - APPOINTMENTS
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            //DOCTOR - PRESCRIPTIONS
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Prescriptions)
                .WithOne(pr => pr.Doctor)
                .HasForeignKey(pr => pr.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            //DOCTOR - PATIENT ASSESSMENTS
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Assessments)
                .WithOne(ass => ass.Doctor)
                .HasForeignKey(ass => ass.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
