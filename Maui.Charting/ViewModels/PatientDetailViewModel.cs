using MedicalCharting.Models;
using MedicalCharting.Services;

namespace Maui.Charting.ViewModels
{
    public class PatientDetailViewModel : BaseViewModel
    {
        private readonly DataStore _store;
        private readonly PatientsViewModel _parent;

        public Patient Patient { get; }

        public IEnumerable<Gender> GenderOptions =>
            Enum.GetValues(typeof(Gender)).Cast<Gender>();

        public PatientDetailViewModel(DataStore store, Patient patient, PatientsViewModel parent)
        {
            _store = store;
            Patient = patient;
            _parent = parent;
        }

        public void Save()
        {
            var existing = _store.Patients.FirstOrDefault(x => x.Id == Patient.Id);
            if (existing != null)
            {
                existing.FirstName = Patient.FirstName;
                existing.LastName = Patient.LastName;
                existing.Address = Patient.Address;
                existing.Race = Patient.Race;
                existing.Gender = Patient.Gender;
                existing.BirthDate = Patient.BirthDate;
            }

            _store.NotifyPatientsChanged();
            _parent.Refresh();
        }
    }
}