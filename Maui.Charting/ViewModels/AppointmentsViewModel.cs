using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MedicalCharting.Models;
using MedicalCharting.Services;
using Maui.Charting.Views;
using Microsoft.Maui.Dispatching;

namespace Maui.Charting.ViewModels
{
    public class AppointmentsViewModel : BaseViewModel
    {
        private readonly DataStore _store;
        private readonly AppointmentService _service;

        public ObservableCollection<Appointment> Appointments { get; } = new();
        public ObservableCollection<Patient> Patients { get; } = new();
        public ObservableCollection<Physician> Physicians { get; } = new();

        // Inline add fields
        public DateTime NewDate { get; set; } = DateTime.Today;
        public TimeSpan NewTime { get; set; } = TimeSpan.FromHours(8);
        public int NewDuration { get; set; } = 30;
        public string NewRoom { get; set; } = string.Empty;

        public Patient? NewSelectedPatient { get; set; }
        public Physician? NewSelectedPhysician { get; set; }

        public List<string> SortOptions { get; } =
            new() { "Start Time", "Patient", "Physician" };

        private string _selectedSortOption = "Start Time";
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                ApplySort();
                OnPropertyChanged();
            }
        }

        private bool _ascending = true;

        public ICommand SortAscCommand { get; }
        public ICommand SortDescCommand { get; }
        public ICommand AddInlineCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public AppointmentsViewModel(DataStore store, AppointmentService service)
        {
            _store = store;
            _service = service;

            SortAscCommand = new Command(() =>
            {
                _ascending = true;
                ApplySort();
            });

            SortDescCommand = new Command(() =>
            {
                _ascending = false;
                ApplySort();
            });

            AddInlineCommand = new Command(AddAppointmentInline);
            DeleteCommand = new Command<Appointment>(DeleteAppointment);
            EditCommand = new Command<Appointment>(OpenEditPage);

            // Subscribe to DataStore events so pickers and list auto-refresh
            _store.PatientsChanged += OnPatientsChanged;
            _store.PhysiciansChanged += OnPhysiciansChanged;
            _store.AppointmentsChanged += OnAppointmentsChanged;

            RefreshAll();
        }

        private void OnPatientsChanged() =>
            MainThread.BeginInvokeOnMainThread(RefreshPatients);

        private void OnPhysiciansChanged() =>
            MainThread.BeginInvokeOnMainThread(RefreshPhysicians);

        private void OnAppointmentsChanged() =>
            MainThread.BeginInvokeOnMainThread(() =>
            {
                RefreshAppointments();
                ApplySort();
            });

        public void Refresh() => RefreshAll();

        private void RefreshAll()
        {
            RefreshPatients();
            RefreshPhysicians();
            RefreshAppointments();
            ApplySort();
        }

        private void RefreshPatients()
        {
            Patients.Clear();
            foreach (var p in _store.Patients)
                Patients.Add(p);
        }

        private void RefreshPhysicians()
        {
            Physicians.Clear();
            foreach (var p in _store.Physicians)
                Physicians.Add(p);
        }

        private void RefreshAppointments()
        {
            Appointments.Clear();
            foreach (var a in _store.Appointments)
                Appointments.Add(a);
        }

        private void AddAppointmentInline()
        {
            if (NewSelectedPatient == null || NewSelectedPhysician == null)
            {
                Application.Current.MainPage.DisplayAlert(
                    "Error", "Patient and physician are required.", "OK");
                return;
            }

            var appt = new Appointment
            {
                Id = _store.Appointments.Any()
                    ? _store.Appointments.Max(a => a.Id) + 1
                    : 1,
                PatientId = NewSelectedPatient.Id,
                PhysicianId = NewSelectedPhysician.Id,
                Room = NewRoom ?? string.Empty,
                Start = NewDate.Date + NewTime,
                DurationMinutes = NewDuration
            };

            if (!_service.TrySchedule(appt, out string msg))
            {
                Application.Current.MainPage.DisplayAlert("Error", msg, "OK");
                return;
            }

            _store.Appointments.Add(appt);
            _store.NotifyAppointmentsChanged();
        }

        private async void OpenEditPage(Appointment appt)
        {
            if (appt == null)
                return;

            var vm = new AppointmentDetailViewModel(_store, _service, appt, this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditAppointmentPage(vm));
        }

        private void DeleteAppointment(Appointment appt)
        {
            if (appt == null)
                return;

            _store.Appointments.Remove(appt);
            _store.NotifyAppointmentsChanged();
        }

        public void ApplySort()
        {
            IEnumerable<Appointment> sorted = SelectedSortOption switch
            {
                "Patient" => Appointments.OrderBy(a => a.PatientId),
                "Physician" => Appointments.OrderBy(a => a.PhysicianId),
                _ => Appointments.OrderBy(a => a.Start)
            };

            if (!_ascending)
                sorted = sorted.Reverse();

            var list = sorted.ToList();
            Appointments.Clear();
            foreach (var item in list)
                Appointments.Add(item);
        }
    }
}
