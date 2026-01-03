using Maui.Charting.ViewModels;

namespace Maui.Charting.Views;

public partial class AppointmentsPage : ContentPage
{
    public AppointmentsPage(AppointmentsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
