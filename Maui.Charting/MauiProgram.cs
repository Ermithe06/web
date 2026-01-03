using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Maui.Charting.ViewModels;
using Maui.Charting.Views;
using MedicalCharting.Services;

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

        // -----------------------
        //  SERVICES (SINGLETONS)
        // -----------------------
        builder.Services.AddSingleton<DataStore>();
        builder.Services.AddSingleton<AppointmentService>();
        builder.Services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("http://10.0.2.2:5000/") // Android emulator -> local machine
                                                           // Use http://localhost:5000/ for Windows desktop only
        });

        builder.Services.AddSingleton<Maui.Charting.Services.MedicalApiClient>();

        // -----------------------
        //  VIEWMODELS (SINGLETONS)
        // -----------------------
        builder.Services.AddSingleton<PatientsViewModel>();
        builder.Services.AddSingleton<PhysiciansViewModel>();
        builder.Services.AddSingleton<AppointmentsViewModel>();

        // -----------------------
        //  PAGES (TRANSIENT)
        // -----------------------
        builder.Services.AddTransient<PatientsPage>();
        builder.Services.AddTransient<PhysiciansPage>();
        builder.Services.AddTransient<AppointmentsPage>();

        builder.Services.AddTransient<EditPatientPage>();
        builder.Services.AddTransient<EditPhysicianPage>();
        builder.Services.AddTransient<EditAppointmentPage>();

        return builder.Build();
    }
}
