using MedicalCharting.Models;
using MedicalCharting.Services;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        // Load persistent store
        var store = DataStore.Persistence.Load();
        var apptService = new AppointmentService(store);

        Console.WriteLine("Medical Charting Console - simple demo");

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1 = Add Patient");
            Console.WriteLine("2 = Add Physician");
            Console.WriteLine("3 = Schedule Appointment");
            Console.WriteLine("4 = List Appointments");
            Console.WriteLine("5 = Save & Exit");

            Console.Write("Choice: ");
            var ch = Console.ReadLine();

            switch (ch)
            {
                case "1": AddPatient(store); break;
                case "2": AddPhysician(store); break;
                case "3": Schedule(store, apptService); break;
                case "4": ListAppointments(store); break;
                case "5":
                    DataStore.Persistence.Save(store);
                    Console.WriteLine("Saved. Exiting.");
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Unknown choice.");
                    break;
            }
        }
    }

    // ------------------------------
    // Add Patient
    // ------------------------------
    static void AddPatient(DataStore store)
    {
        var p = new Patient();

        Console.Write("First name: ");
        p.FirstName = Console.ReadLine() ?? "";

        Console.Write("Last name: ");
        p.LastName = Console.ReadLine() ?? "";

        Console.Write("Address: ");
        p.Address = Console.ReadLine() ?? "";

        Console.Write("Birthdate (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out var d))
            p.BirthDate = d;

        Console.Write("Race: ");
        p.Race = Console.ReadLine() ?? "";

        Console.Write("Gender (Male/Female/Other): ");
        var g = Console.ReadLine();
        p.Gender = Enum.TryParse<Gender>(g, true, out var gender)
            ? gender
            : Gender.Unknown;

        p.Id = store.Patients.Any() ? store.Patients.Max(x => x.Id) + 1 : 1;

        store.Patients.Add(p);
        store.NotifyPatientsChanged();

        Console.WriteLine($"Added patient #{p.Id}: {p.FullName}");
    }

    // ------------------------------
    // Add Physician
    // ------------------------------
    static void AddPhysician(DataStore store)
    {
        var ph = new Physician();

        Console.Write("First name: ");
        ph.FirstName = Console.ReadLine() ?? "";

        Console.Write("Last name: ");
        ph.LastName = Console.ReadLine() ?? "";

        Console.Write("License number: ");
        ph.LicenseNumber = Console.ReadLine() ?? "";

        Console.Write("Graduation date (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out var d))
            ph.GraduationDate = d;

        Console.Write("Specialization: ");
        ph.Specialization = Console.ReadLine() ?? "";

        ph.Id = store.Physicians.Any() ? store.Physicians.Max(x => x.Id) + 1 : 1;

        store.Physicians.Add(ph);
        store.NotifyPhysiciansChanged();

        Console.WriteLine($"Added physician #{ph.Id}: {ph.FullName}");
    }

    // ------------------------------
    // Schedule Appointment
    // ------------------------------
    static void Schedule(DataStore store, AppointmentService apptService)
    {
        Console.Write("Physician Id: ");
        if (!int.TryParse(Console.ReadLine(), out var pid))
        {
            Console.WriteLine("Bad id");
            return;
        }

        Console.Write("Patient Id: ");
        if (!int.TryParse(Console.ReadLine(), out var patId))
        {
            Console.WriteLine("Bad id");
            return;
        }

        Console.Write("Start (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var start))
        {
            Console.WriteLine("Bad date");
            return;
        }

        Console.Write("Duration minutes (default 30): ");
        var durStr = Console.ReadLine();
        int minutes = 30;
        if (int.TryParse(durStr, out var m)) minutes = m;

        var appt = new Appointment
        {
            PhysicianId = pid,
            PatientId = patId,
            Start = start,
            DurationMinutes = minutes,
            Room = "A1"
        };

        if (apptService.TrySchedule(appt, out var msg))
        {
            store.Appointments.Add(appt);
            store.NotifyAppointmentsChanged();
            DataStore.Persistence.Save(store);

            Console.WriteLine($"Scheduled appointment at {appt.Start}");
        }
        else
        {
            Console.WriteLine("Failed to schedule: " + msg);
        }
    }

    // ------------------------------
    // List Appointments
    // ------------------------------
    static void ListAppointments(DataStore store)
    {
        if (!store.Appointments.Any())
        {
            Console.WriteLine("No appointments.");
            return;
        }

        foreach (var a in store.Appointments.OrderBy(x => x.Start))
        {
            var ph = store.Physicians.FirstOrDefault(p => p.Id == a.PhysicianId);
            var pat = store.Patients.FirstOrDefault(p => p.Id == a.PatientId);

            Console.WriteLine(
                $"{a.Id}: {a.Start:yyyy-MM-dd HH:mm} - {a.End:HH:mm}  " +
                $"Dr: {ph?.FullName ?? "?"}  Patient: {pat?.FullName ?? "?"}  Room: {a.Room}"
            );
        }
    }
}