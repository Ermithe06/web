using Maui.Charting.Services;
using Maui.Charting.Views;
using MedicalCharting.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maui.Charting.ViewModels
{
    public class PhysiciansViewModel : BaseViewModel
    {
        private readonly MedicalApiClient _api;

        public ObservableCollection<Physician> Physicians { get; } = new();

        // Add Physician fields
        public string NewFirstName { get; set; } = "";
        public string NewLastName { get; set; } = "";
        public string NewLicense { get; set; } = "";
        public string NewSpecialization { get; set; } = "";
        public DateTime NewGraduationDate { get; set; } = DateTime.Today;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public PhysiciansViewModel(MedicalApiClient api)
        {
            _api = api;

            AddCommand = new Command(async () => await AddPhysician());
            DeleteCommand = new Command<Physician>(async (p) => await DeletePhysician(p));
            EditCommand = new Command<Physician>(OpenEditPage);

            Refresh();
        }

        // Keep Refresh() because pages call it
        public void Refresh() => _ = LoadFromApi();

        public async Task LoadFromApi()
        {
            Physicians.Clear();
            var data = await _api.GetPhysicians();
            foreach (var p in data)
                Physicians.Add(p);
        }

        private async Task AddPhysician()
        {
            if (string.IsNullOrWhiteSpace(NewFirstName) ||
                string.IsNullOrWhiteSpace(NewLastName) ||
                string.IsNullOrWhiteSpace(NewLicense))
            {
                await Application.Current!.MainPage!
                    .DisplayAlert("Error", "All required fields must be filled.", "OK");
                return;
            }

            var physician = new Physician
            {
                FirstName = NewFirstName,
                LastName = NewLastName,
                LicenseNumber = NewLicense,
                Specialization = NewSpecialization,
                GraduationDate = NewGraduationDate
            };

            await _api.AddPhysician(physician);
            await LoadFromApi();
            ClearForm();
        }

        private async Task DeletePhysician(Physician? physician)
        {
            if (physician == null) return;

            await _api.DeletePhysician(physician.Id);
            await LoadFromApi();
        }

        private async void OpenEditPage(Physician? physician)
        {
            if (physician == null) return;

            var vm = new PhysicianDetailViewModel(_api, physician, this);
            await Application.Current!.MainPage!
                .Navigation.PushAsync(new EditPhysicianPage(vm));
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
