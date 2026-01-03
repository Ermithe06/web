using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Maui.Charting.ViewModels;
using Maui.Charting.Views;
using Maui.Charting.Services;

namespace Maui.Charting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // =======================
        // SERVICES
        // =======================

        builder.Services.AddSingleton(new HttpClient
        {
            // Android emulator → local machine
            BaseAddress = new Uri("http://10.0.2.2:5000/")
            // Windows desktop ONLY: http://localhost:5000/
        });

        builder.Services.AddSingleton<MedicalApiClient>();

        // =======================
        // VIEWMODELS
        // =======================

        builder.Services.AddSingleton<PatientsViewModel>();
        builder.Services.AddSingleton<PhysiciansViewModel>();
        builder.Services.AddSingleton<AppointmentsViewModel>();

        // =======================
        // PAGES
        // =======================

        builder.Services.AddTransient<PatientsPage>();
        builder.Services.AddTransient<PhysiciansPage>();
        builder.Services.AddTransient<AppointmentsPage>();

        builder.Services.AddTransient<EditPatientPage>();
        builder.Services.AddTransient<EditPhysicianPage>();
        builder.Services.AddTransient<EditAppointmentPage>();

        return builder.Build();
    }
}
