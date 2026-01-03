using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MedicalCharting.Models;
using MedicalCharting.Services;

namespace Maui.Charting.ViewModels
{
    public class AppointmentDetailViewModel : BaseViewModel
    {
        private readonly DataStore _store;
        private readonly AppointmentService _service;
        private readonly AppointmentsViewModel _parent;

        public Appointment Appointment { get; }

        // pickers
        public ObservableCollection<Patient> Patients { get; } = new();
        public ObservableCollection<Physician> Physicians { get; } = new();

        private Patient? _selectedPatient;
        public Patient? SelectedPatient
        {
            get => _selectedPatient;
            set { _selectedPatient = value; OnPropertyChanged(); }
        }

        private Physician? _selectedPhysician;
        public Physician? SelectedPhysician
        {
            get => _selectedPhysician;
            set { _selectedPhysician = value; OnPropertyChanged(); }
        }

        // basic info
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int DurationMinutes { get; set; }
        public string Room { get; set; } = string.Empty;

        // diagnoses & treatments
        public ObservableCollection<string> Diagnoses { get; } = new();
        public ObservableCollection<Treatment> Treatments { get; } = new();

        public string NewDiagnosis { get; set; } = string.Empty;
        public string NewTreatmentName { get; set; } = string.Empty;
        public decimal NewTreatmentCost { get; set; }

        // commands
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddDiagnosisCommand { get; }
        public ICommand RemoveDiagnosisCommand { get; }
        public ICommand AddTreatmentCommand { get; }
        public ICommand RemoveTreatmentCommand { get; }

        public AppointmentDetailViewModel(
            DataStore store,
            AppointmentService service,
            Appointment appointment,
            AppointmentsViewModel parent)
        {
            _store = store;
            _service = service;
            _parent = parent;
            Appointment = appointment;

            // populate collections
            foreach (var p in _store.Patients)
                Patients.Add(p);
            foreach (var ph in _store.Physicians)
                Physicians.Add(ph);

            SelectedPatient = _store.Patients.FirstOrDefault(p => p.Id == appointment.PatientId);
            SelectedPhysician = _store.Physicians.FirstOrDefault(p => p.Id == appointment.PhysicianId);

            Date = appointment.Start.Date;
            Time = appointment.Start.TimeOfDay;
            DurationMinutes = appointment.DurationMinutes;
            Room = appointment.Room;

            foreach (var d in appointment.Diagnoses)
                Diagnoses.Add(d);

            foreach (var t in appointment.Treatments)
                Treatments.Add(t);

            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);

            AddDiagnosisCommand = new Command(AddDiagnosis);
            RemoveDiagnosisCommand = new Command<string>(RemoveDiagnosis);

            AddTreatmentCommand = new Command(AddTreatment);
            RemoveTreatmentCommand = new Command<Treatment>(RemoveTreatment);
        }

        // ---------- Save / Delete ----------

        private async void Save()
        {
            if (SelectedPatient == null || SelectedPhysician == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error", "Patient and physician are required.", "OK");
                return;
            }

            // Push edited values back onto the Appointment model
            Appointment.PatientId = SelectedPatient.Id;
            Appointment.PhysicianId = SelectedPhysician.Id;
            Appointment.Start = Date.Date + Time;
            Appointment.DurationMinutes = DurationMinutes;
            Appointment.Room = Room ?? string.Empty;

            // sync diagnoses & treatments
            Appointment.Diagnoses.Clear();
            Appointment.Diagnoses.AddRange(Diagnoses);

            Appointment.Treatments.Clear();
            Appointment.Treatments.AddRange(Treatments);

            if (!_service.TrySchedule(Appointment, out string msg, isEdit: true)
)
            {
                await Application.Current.MainPage.DisplayAlert("Error", msg, "OK");
                return;
            }

            // notify & refresh parent list
            _store.NotifyAppointmentsChanged();
            _parent.Refresh();

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void Delete()
        {
            _store.Appointments.Remove(Appointment);
            _store.NotifyAppointmentsChanged();
            _parent.Refresh();

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        // ---------- Diagnoses ----------

        private void AddDiagnosis()
        {
            if (string.IsNullOrWhiteSpace(NewDiagnosis))
                return;

            Diagnoses.Add(NewDiagnosis);
            NewDiagnosis = string.Empty;
            OnPropertyChanged(nameof(NewDiagnosis));
        }

        private void RemoveDiagnosis(string d)
        {
            if (d == null) return;
            Diagnoses.Remove(d);
        }

        // ---------- Treatments ----------

        private void AddTreatment()
        {
            if (string.IsNullOrWhiteSpace(NewTreatmentName))
                return;

            var t = new Treatment
            {
                Name = NewTreatmentName,
                Cost = NewTreatmentCost
            };

            Treatments.Add(t);

            NewTreatmentName = string.Empty;
            NewTreatmentCost = 0m;
            OnPropertyChanged(nameof(NewTreatmentName));
            OnPropertyChanged(nameof(NewTreatmentCost));
        }

        private void RemoveTreatment(Treatment t)
        {
            if (t == null) return;
            Treatments.Remove(t);
        }
    }
}
