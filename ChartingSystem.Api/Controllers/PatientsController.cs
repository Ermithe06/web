using Microsoft.AspNetCore.Mvc;
using MedicalCharting.Models;
using ChartingSystem.Api.Repositories;

namespace ChartingSystem.Api.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() =>
        Ok(PatientRepository.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var patient = PatientRepository.GetById(id);
        return patient == null ? NotFound() : Ok(patient);
    }

    [HttpPost]
    public IActionResult Create(Patient patient)
    {
        PatientRepository.Add(patient);
        return Ok(patient);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Patient patient)
    {
        return PatientRepository.Update(id, patient)
            ? Ok(patient)
            : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return PatientRepository.Delete(id)
            ? Ok()
            : NotFound();
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string q) =>
        Ok(PatientRepository.Search(q));
}
