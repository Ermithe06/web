using System.ComponentModel;
using System.Runtime.CompilerServices;
using MedicalCharting.Models;
using MedicalCharting.Services;

namespace Maui.Charting.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}


