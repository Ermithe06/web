using MedicalCharting.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MedicalCharting.Services
{
    public class DataStore
    {
        public List<Patient> Patients { get; set; } = new();
        public List<Physician> Physicians { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();

        // 🔥 Events so ViewModels update automatically
        public event Action? PatientsChanged;
        public event Action? PhysiciansChanged;
        public event Action? AppointmentsChanged;

        public void NotifyPatientsChanged() => PatientsChanged?.Invoke();
        public void NotifyPhysiciansChanged() => PhysiciansChanged?.Invoke();
        public void NotifyAppointmentsChanged() => AppointmentsChanged?.Invoke();

        public DataStore()
        {
            SeedPatients();
            SeedPhysicians();
            SeedAppointments();

            // Notify UI initial load
            NotifyPatientsChanged();
            NotifyPhysiciansChanged();
            NotifyAppointmentsChanged();
        }

        private void SeedPatients()
        {
            Patients.Add(new Patient
            {
                Id = 1,
                FirstName = "Emma",
                LastName = "Brown",
                BirthDate = new DateTime(1998, 3, 12),
                Address = "123 Elm St",
                Race = "Black",
                Gender = Gender.Female
            });

            Patients.Add(new Patient
            {
                Id = 2,
                FirstName = "Jacob",
                LastName = "Rivera",
                BirthDate = new DateTime(2005, 10, 8),
                Address = "44 Magnolia Ave",
                Race = "Hispanic",
                Gender = Gender.Male
            });
        }

        private void SeedPhysicians()
        {
            Physicians.Add(new Physician
            {
                Id = 1,
                FirstName = "Sarah",
                LastName = "Miller",
                LicenseNumber = "FL12345",
                GraduationDate = new DateTime(2018, 5, 10),
                Specialization = "Pediatrics"
            });

            Physicians.Add(new Physician
            {
                Id = 2,
                FirstName = "David",
                LastName = "Nguyen",
                LicenseNumber = "FL67890",
                GraduationDate = new DateTime(2015, 6, 2),
                Specialization = "Dermatology"
            });
        }

        private void SeedAppointments()
        {
            Appointments.Add(new Appointment
            {
                Id = 1,
                PatientId = 1,
                PhysicianId = 1,
                Start = DateTime.Today.AddHours(10),
                DurationMinutes = 30,
                Room = "A1"
            });

            Appointments.Add(new Appointment
            {
                Id = 2,
                PatientId = 2,
                PhysicianId = 2,
                Start = DateTime.Today.AddHours(14),
                DurationMinutes = 45,
                Room = "B2"
            });
        }

        // --------------------------------------------------------
        // 🔥 PERSISTENCE SYSTEM (CORRECT — ONLY ONE VERSION)
        // --------------------------------------------------------
        public static class Persistence
        {
            private static readonly JsonSerializerOptions _opts =
                new JsonSerializerOptions { WriteIndented = true };

            public static void Save(DataStore store, string folder = "data")
            {
                Directory.CreateDirectory(folder);

                File.WriteAllText(Path.Combine(folder, "patients.json"),
                    JsonSerializer.Serialize(store.Patients, _opts));

                File.WriteAllText(Path.Combine(folder, "physicians.json"),
                    JsonSerializer.Serialize(store.Physicians, _opts));

                File.WriteAllText(Path.Combine(folder, "appointments.json"),
                    JsonSerializer.Serialize(store.Appointments, _opts));
            }

            public static DataStore Load(string folder = "data")
            {
                var ds = new DataStore();

                if (!Directory.Exists(folder))
                    return ds;

                string ReadFile(string name)
                {
                    string path = Path.Combine(folder, name);
                    return File.Exists(path) ? File.ReadAllText(path) : null;
                }

                var pJson = ReadFile("patients.json");
                if (pJson != null)
                    ds.Patients = JsonSerializer.Deserialize<List<Patient>>(pJson) ?? new();

                var phJson = ReadFile("physicians.json");
                if (phJson != null)
                    ds.Physicians = JsonSerializer.Deserialize<List<Physician>>(phJson) ?? new();

                var aJson = ReadFile("appointments.json");
                if (aJson != null)
                    ds.Appointments = JsonSerializer.Deserialize<List<Appointment>>(aJson) ?? new();

                // Notify after loading
                ds.NotifyPatientsChanged();
                ds.NotifyPhysiciansChanged();
                ds.NotifyAppointmentsChanged();

                return ds;
            }
        }
    }
}