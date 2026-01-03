using Maui.Charting.Views;
using MedicalCharting.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Maui.Charting.Services;

namespace Maui.Charting.ViewModels;

public class PatientsViewModel : BaseViewModel
{
    private readonly MedicalApiClient _api;

    public ObservableCollection<Patient> Patients { get; } = new();

    public string NewFirstName { get; set; } = "";
    public string NewLastName { get; set; } = "";
    public string NewAddress { get; set; } = "";
    public DateTime NewBirthDate { get; set; } = DateTime.Today;
    public string NewRace { get; set; } = "";
    public Gender NewGender { get; set; } = Gender.Unknown;

    public IEnumerable<Gender> GenderOptions =>
        Enum.GetValues(typeof(Gender)).Cast<Gender>();

    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }

    public PatientsViewModel(MedicalApiClient api)
    {
        _api = api;

        AddCommand = new Command(async () => await AddPatient());
        DeleteCommand = new Command<Patient>(async (p) => await DeletePatient(p));
        EditCommand = new Command<Patient>(OpenEditPage);
    }

    // keep a Refresh() method because other code calls it
    public void Refresh() => _ = LoadFromApi();

    public async Task LoadFromApi()
    {
        Patients.Clear();
        var data = await _api.GetPatients();
        foreach (var p in data)
            Patients.Add(p);
    }

    private async Task AddPatient()
    {
        if (string.IsNullOrWhiteSpace(NewFirstName) || string.IsNullOrWhiteSpace(NewLastName))
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Name fields are required.", "OK");
            return;
        }

        var patient = new Patient
        {
            FirstName = NewFirstName,
            LastName = NewLastName,
            Address = NewAddress,
            BirthDate = NewBirthDate,
            Race = NewRace,
            Gender = NewGender
        };

        await _api.AddPatient(patient);
        await LoadFromApi();
        ClearForm();
    }

    private async Task DeletePatient(Patient? p)
    {
        if (p == null) return;
        await _api.DeletePatient(p.Id);
        await LoadFromApi();
    }

    private async void OpenEditPage(Patient? patient)
    {
        if (patient == null) return;
        var vm = new PatientDetailViewModel(_api, patient, this);
        await Application.Current!.MainPage!.Navigation.PushAsync(new EditPatientPage(vm));
    }

    private void ClearForm()
    {
        NewFirstName = "";
        NewLastName = "";
        NewAddress = "";
        NewRace = "";
        NewGender = Gender.Unknown;
        NewBirthDate = DateTime.Today;

        OnPropertyChanged(nameof(NewFirstName));
        OnPropertyChanged(nameof(NewLastName));
        OnPropertyChanged(nameof(NewAddress));
        OnPropertyChanged(nameof(NewRace));
        OnPropertyChanged(nameof(NewGender));
        OnPropertyChanged(nameof(NewBirthDate));
    }
}
