using MedicalCharting.Models;
using MedicalCharting.Services;

namespace Maui.Charting.ViewModels
{
    public class PhysicianDetailViewModel : BaseViewModel
    {
        private readonly DataStore _store;
        private readonly PhysiciansViewModel _parent;

        public Physician Physician { get; }

        public PhysicianDetailViewModel(DataStore store, Physician physician, PhysiciansViewModel parent)
        {
            _store = store;
            Physician = physician;
            _parent = parent;
        }

        public void Save()
        {
            var existing = _store.Physicians.FirstOrDefault(p => p.Id == Physician.Id);

            if (existing != null)
            {
                existing.FirstName = Physician.FirstName;
                existing.LastName = Physician.LastName;
                existing.LicenseNumber = Physician.LicenseNumber;
                existing.GraduationDate = Physician.GraduationDate;
                existing.Specialization = Physician.Specialization;
            }

            _store.NotifyPhysiciansChanged();
            _parent.Refresh();
        }
    }
}