using MedicalCharting.Models;
using Maui.Charting.Services;
using System.Windows.Input;

namespace Maui.Charting.ViewModels;

public class PhysicianDetailViewModel : BaseViewModel
{
    private readonly MedicalApiClient _api;
    private readonly PhysiciansViewModel _parent;

    public Physician Physician { get; }

    public ICommand SaveCommand { get; }

    public PhysicianDetailViewModel(
        MedicalApiClient api,
        Physician physician,
        PhysiciansViewModel parent)
    {
        _api = api;
        Physician = physician;
        _parent = parent;

        SaveCommand = new Command(async () => await Save());
    }

    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Physician.FirstName) ||
            string.IsNullOrWhiteSpace(Physician.LastName) ||
            string.IsNullOrWhiteSpace(Physician.LicenseNumber))
        {
            await Application.Current!.MainPage!
                .DisplayAlert("Error", "First name, last name, and license are required.", "OK");
            return;
        }

        // PUT to API
        await _api.UpdatePhysician(Physician.Id, Physician);

        // Refresh parent list from API
        _parent.Refresh();

        // Navigate back
        await Application.Current!.MainPage!.Navigation.PopAsync();
    }
}
