using MedicalCharting.Models;
using System.Text;
using System.Text.Json;

namespace Maui.Charting.Services;

public class MedicalApiClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public MedicalApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Patient>> GetPatients(string? q = null)
    {
        var url = "api/patients" + (string.IsNullOrWhiteSpace(q) ? "" : $"?q={Uri.EscapeDataString(q)}");
        var json = await _http.GetStringAsync(url);
        return JsonSerializer.Deserialize<List<Patient>>(json, _json) ?? new();
    }

    public async Task AddPatient(Patient p)
    {
        var json = JsonSerializer.Serialize(p, _json);
        var resp = await _http.PostAsync("api/patients",
            new StringContent(json, Encoding.UTF8, "application/json"));
        resp.EnsureSuccessStatusCode();
    }

    public async Task UpdatePatient(int id, Patient p)
    {
        var json = JsonSerializer.Serialize(p, _json);
        var resp = await _http.PutAsync($"api/patients/{id}",
            new StringContent(json, Encoding.UTF8, "application/json"));
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeletePatient(int id)
    {
        var resp = await _http.DeleteAsync($"api/patients/{id}");
        resp.EnsureSuccessStatusCode();
    }
}
