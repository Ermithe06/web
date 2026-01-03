using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MedicalCharting.Models
{
    public class Physician : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string _firstName = "";
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName == value) return;
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(FullName)); // force UI refresh
            }
        }

        private string _lastName = "";
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(FullName)); // force UI refresh
            }
        }

        private string _licenseNumber = "";
        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                if (_licenseNumber == value) return;
                _licenseNumber = value;
                OnPropertyChanged(nameof(LicenseNumber));
            }
        }

        private DateTime _graduationDate;
        public DateTime GraduationDate
        {
            get => _graduationDate;
            set
            {
                if (_graduationDate == value) return;
                _graduationDate = value;
                OnPropertyChanged(nameof(GraduationDate));
            }
        }

        private string _specialization = "";
        public string Specialization
        {
            get => _specialization;
            set
            {
                if (_specialization == value) return;
                _specialization = value;
                OnPropertyChanged(nameof(Specialization));
            }
        }

        // Computed property — MUST be refreshed manually
        public string FullName => $"{FirstName} {LastName}";

        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
