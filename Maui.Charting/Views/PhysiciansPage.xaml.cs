using Maui.Charting.ViewModels;

namespace Maui.Charting.Views
{
    public partial class PhysiciansPage : ContentPage
    {
        public PhysiciansPage(PhysiciansViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}