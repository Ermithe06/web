using Maui.Charting.ViewModels;

namespace Maui.Charting.Views;

public partial class EditPhysicianPage : ContentPage
{
    public EditPhysicianPage(PhysicianDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}