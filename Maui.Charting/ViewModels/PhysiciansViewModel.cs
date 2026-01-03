using Maui.Charting.Views;
using MedicalCharting.Models;
using MedicalCharting.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maui.Charting.ViewModels
{
    public class PhysiciansViewModel : BaseViewModel
    {
        private readonly DataStore _store;

        public ObservableCollection<Physician> Physicians { get; } = new();

        public string NewFirstName { get; set; } = "";
        public string NewLastName { get; set; } = "";
        public string NewLicense { get; set; } = "";
        public string NewSpecialization { get; set; } = "";
        public DateTime NewGraduationDate { get; set; } = DateTime.Today;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public PhysiciansViewModel(DataStore store)
        {
            _store = store;

            _store.PhysiciansChanged += Refresh;

            AddCommand = new Command(AddPhysician);
            DeleteCommand = new Command<Physician>(DeletePhysician);
            EditCommand = new Command<Physician>(OpenEditPage);

            Refresh();
        }

        public void Refresh()
        {
            Physicians.Clear();
            foreach (var p in _store.Physicians)
                Physicians.Add(p);
        }

        private void AddPhysician()
        {
            if (string.IsNullOrWhiteSpace(NewFirstName) ||
                string.IsNullOrWhiteSpace(NewLastName) ||
                string.IsNullOrWhiteSpace(NewLicense))
            {
                Application.Current.MainPage.DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            var physician = new Physician
            {
                Id = _store.Physicians.Any() ? _store.Physicians.Max(p => p.Id) + 1 : 1,
                FirstName = NewFirstName,
                LastName = NewLastName,
                LicenseNumber = NewLicense,
                GraduationDate = NewGraduationDate,
                Specialization = NewSpecialization
            };

            _store.Physicians.Add(physician);
            _store.NotifyPhysiciansChanged();

            ClearForm();
        }

        private void DeletePhysician(Physician physician)
        {
            if (physician == null) return;

            _store.Physicians.Remove(physician);
            _store.NotifyPhysiciansChanged();
        }

        private async void OpenEditPage(Physician physician)
        {
            if (physician == null) return;

            var vm = new PhysicianDetailViewModel(_store, physician, this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditPhysicianPage(vm));
        }

        private void ClearForm()
        {
            NewFirstName = "";
            NewLastName = "";
            NewLicense = "";
            NewSpecialization = "";
            NewGraduationDate = DateTime.Today;

            OnPropertyChanged(nameof(NewFirstName));
            OnPropertyChanged(nameof(NewLastName));
            OnPropertyChanged(nameof(NewLicense));
            OnPropertyChanged(nameof(NewSpecialization));
            OnPropertyChanged(nameof(NewGraduationDate));
        }
    }
}