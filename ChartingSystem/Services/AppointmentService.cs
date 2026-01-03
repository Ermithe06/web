using System;
using System.Linq;
using MedicalCharting.Models;

namespace MedicalCharting.Services
{
    public class AppointmentService
    {
        private readonly DataStore _store;
        private readonly TimeSpan _open = TimeSpan.FromHours(8);
        private readonly TimeSpan _close = TimeSpan.FromHours(17);

        public AppointmentService(DataStore store)
        {
            _store = store;
        }

        // Used by create + edit
        public bool TrySchedule(Appointment appt, out string message, bool isEdit = false)
        {
            // Validate Mon-Fri
            if (appt.Start.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                message = "Appointments allowed only Monday–Friday.";
                return false;
            }

            // Validate time
            if (appt.Start.TimeOfDay < _open || appt.End.TimeOfDay > _close)
            {
                message = "Appointment must be between 08:00–17:00.";
                return false;
            }

            // Validate relationships
            if (!_store.Physicians.Any(p => p.Id == appt.PhysicianId))
            {
                message = "Physician not found.";
                return false;
            }

            if (!_store.Patients.Any(p => p.Id == appt.PatientId))
            {
                message = "Patient not found.";
                return false;
            }

            // Conflict check
            var conflicts = _store.Appointments
                .Where(a => a.PhysicianId == appt.PhysicianId);

            if (isEdit)
                conflicts = conflicts.Where(a => a.Id != appt.Id);

            foreach (var a in conflicts)
            {
                if (appt.Start < a.End && a.Start < appt.End)
                {
                    message = $"Conflict with appointment #{a.Id}.";
                    return false;
                }
            }

            message = "OK";
            return true;
        }
    }
}
