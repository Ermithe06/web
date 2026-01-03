using Maui.Charting.ViewModels;

namespace Maui.Charting.Views;

public partial class PatientsPage : ContentPage
{
    // ONLY ONE constructor!
    public PatientsPage(PatientsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PatientsViewModel vm)
            await vm.LoadFromApi();
    }

}
