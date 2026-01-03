using Maui.Charting.ViewModels;

namespace Maui.Charting.Views;

public partial class EditPatientPage : ContentPage
{
    public EditPatientPage(PatientDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}