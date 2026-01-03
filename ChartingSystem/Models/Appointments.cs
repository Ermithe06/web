using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalCharting.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }

        public DateTime Start { get; set; }
        public int DurationMinutes { get; set; }
        public string Room { get; set; } = "";

        // Assignment 3
        public List<string> Diagnoses { get; set; } = new();
        public List<Treatment> Treatments { get; set; } = new();

        public DateTime End => Start.AddMinutes(DurationMinutes);
        public bool IsToday => Start.Date == DateTime.Today;

        public decimal TotalTreatmentCost =>
            Treatments?.Sum(t => t.Cost) ?? 0m;
    }
}