using MedicalCharting.Models;
using System.Xml.Linq;

namespace ChartingSystem.Api.Repositories;

public static class PatientRepository
{
    private static readonly List<Patient> _patients = new();

    public static IEnumerable<Patient> GetAll() => _patients;

    public static Patient? GetById(int id) =>
        _patients.FirstOrDefault(p => p.Id == id);

    public static void Add(Patient patient)
    {
        patient.Id = _patients.Any() ? _patients.Max(p => p.Id) + 1 : 1;
        _patients.Add(patient);
    }

    public static bool Update(int id, Patient updated)
    {
        var existing = GetById(id);
        if (existing == null) return false;

        existing.FirstName = updated.FirstName;
        existing.LastName = updated.LastName;
        existing.Address = updated.Address;
        existing.BirthDate = updated.BirthDate;
        existing.Race = updated.Race;
        existing.Gender = updated.Gender;

        return true;
    }

    public static bool Delete(int id)
    {
        var patient = GetById(id);
        if (patient == null) return false;

        _patients.Remove(patient);
        return true;
    }

    public static IEnumerable<Patient> Search(string query) =>
        _patients.Where(p =>
            p.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.LastName.Contains(query, StringComparison.OrdinalIgnoreCase));
}

