using MedicalCharting.Models;
using Maui.Charting.Services;
using System.Windows.Input;

namespace Maui.Charting.ViewModels;

public class PatientDetailViewModel : BaseViewModel
{
    private readonly MedicalApiClient _api;
    private readonly PatientsViewModel _parent;

    public Patient Patient { get; }

    public IEnumerable<Gender> GenderOptions =>
        Enum.GetValues(typeof(Gender)).Cast<Gender>();

    public ICommand SaveCommand { get; }

    public PatientDetailViewModel(
        MedicalApiClient api,
        Patient patient,
        PatientsViewModel parent)
    {
        _api = api;
        Patient = patient;
        _parent = parent;

        SaveCommand = new Command(async () => await Save());
    }

    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Patient.FirstName) ||
            string.IsNullOrWhiteSpace(Patient.LastName))
        {
            await Application.Current!.MainPage!
                .DisplayAlert("Error", "Name fields are required.", "OK");
            return;
        }

        // PUT to API
        await _api.UpdatePatient(Patient.Id, Patient);

        // Refresh parent list from API
        _parent.Refresh();

        // Go back
        await Application.Current!.MainPage!.Navigation.PopAsync();
    }
}