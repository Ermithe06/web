using MedicalCharting.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.eCommerce.Utilities; // WebRequestHandler namespace

namespace MedicalCharting.Services
{
    public class PatientsApi
    {
        private readonly WebRequestHandler _api = new();

        public async Task<List<Patient>> GetPatients()
        {
            var json = await _api.Get("/api/patients");
            return JsonConvert.DeserializeObject<List<Patient>>(json) ?? new();
        }

        public async Task<Patient?> GetPatient(int id)
        {
            var json = await _api.Get($"/api/patients/{id}");
            return JsonConvert.DeserializeObject<Patient>(json);
        }

        public async Task AddPatient(Patient patient)
        {
            await _api.Post("/api/patients", patient);
        }

        public async Task UpdatePatient(int id, Patient patient)
        {
            await _api.Post($"/api/patients/{id}", patient);
        }

        public async Task DeletePatient(int id)
        {
            await _api.Delete($"/api/patients/{id}");
        }

        public async Task<List<Patient>> Search(string query)
        {
            var json = await _api.Get($"/api/patients/search?q={query}");
            return JsonConvert.DeserializeObject<List<Patient>>(json) ?? new();
        }
    }
}
