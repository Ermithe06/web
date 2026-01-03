using Maui.Charting.ViewModels;

namespace Maui.Charting.Views
{
    public partial class EditAppointmentPage : ContentPage
    {
        public EditAppointmentPage(AppointmentDetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}