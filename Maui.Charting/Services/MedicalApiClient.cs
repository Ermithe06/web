using MedicalCharting.Models;
using System.Text;
using System.Text.Json;

namespace Maui.Charting.Services
{
    public class MedicalApiClient
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _json =
            new(JsonSerializerDefaults.Web);

        public MedicalApiClient(HttpClient http)
        {
            _http = http;
        }

        /* ============================
         * PATIENTS
         * ============================ */

        public async Task<List<Patient>> GetPatients(string? q = null)
        {
            var url = "api/patients" +
                (string.IsNullOrWhiteSpace(q) ? "" : $"?q={Uri.EscapeDataString(q)}");

            var json = await _http.GetStringAsync(url);
            return JsonSerializer.Deserialize<List<Patient>>(json, _json) ?? new();
        }

        public async Task AddPatient(Patient patient)
        {
            var json = JsonSerializer.Serialize(patient, _json);
            var resp = await _http.PostAsync(
                "api/patients",
                new StringContent(json, Encoding.UTF8, "application/json"));

            resp.EnsureSuccessStatusCode();
        }

        public async Task UpdatePatient(int id, Patient patient)
        {
            var json = JsonSerializer.Serialize(patient, _json);
            var resp = await _http.PutAsync(
                $"api/patients/{id}",
                new StringContent(json, Encoding.UTF8, "application/json"));

            resp.EnsureSuccessStatusCode();
        }

        public async Task DeletePatient(int id)
        {
            var resp = await _http.DeleteAsync($"api/patients/{id}");
            resp.EnsureSuccessStatusCode();
        }

        /* ============================
         * PHYSICIANS
         * ============================ */

        public async Task<List<Physician>> GetPhysicians(string? q = null)
        {
            var url = "api/physicians" +
                (string.IsNullOrWhiteSpace(q) ? "" : $"?q={Uri.EscapeDataString(q)}");

            var json = await _http.GetStringAsync(url);
            return JsonSerializer.Deserialize<List<Physician>>(json, _json) ?? new();
        }

        public async Task AddPhysician(Physician physician)
        {
            var json = JsonSerializer.Serialize(physician, _json);
            var resp = await _http.PostAsync(
                "api/physicians",
                new StringContent(json, Encoding.UTF8, "application/json"));

            resp.EnsureSuccessStatusCode();
        }

        public async Task UpdatePhysician(int id, Physician physician)
        {
            var json = JsonSerializer.Serialize(physician, _json);
            var resp = await _http.PutAsync(
                $"api/physicians/{id}",
                new StringContent(json, Encoding.UTF8, "application/json"));

            resp.EnsureSuccessStatusCode();
        }

        public async Task DeletePhysician(int id)
        {
            var resp = await _http.DeleteAsync($"api/physicians/{id}");
            resp.EnsureSuccessStatusCode();
        }

        /* ============================
         * APPOINTMENTS
         * ============================ */

        public async Task<List<Appointment>> GetAppointments(string? q = null)
        {
            var url = "api/appointments" +
                (string.IsNullOrWhiteSpace(q) ? "" : $"?q={Uri.EscapeDataString(q)}");

            var json = await _http.GetStringAsync(url);
            return JsonSerializer.Deserialize<List<Appointment>>(json, _json) ?? new();
        }

        public async Task AddAppointment(Appointment appointment)
        {
            var json = JsonSerializer.Serialize(appointment, _json);
            var resp = await _http.PostAsync(
                "api/appointments",
                new StringContent(json, Encoding.UTF8, "application/json"));

            resp.EnsureSuccessStatusCode();
        }

        public async Task UpdateAppointment(int id, Appointment appointment)
        {
            var json = JsonSerializer.Serialize(appointment, _json);
            var resp = await _http.PutAsync(
                $"api/appointments/{id}",
                new StringContent(json, Encoding.UTF8, "application/json"));

            resp.EnsureSuccessStatusCode();
        }

        public async Task DeleteAppointment(int id)
        {
            var resp = await _http.DeleteAsync($"api/appointments/{id}");
            resp.EnsureSuccessStatusCode();
        }
    }
}
