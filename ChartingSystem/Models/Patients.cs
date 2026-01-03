using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MedicalCharting.Models
{
    public class Patient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _id;
        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        private string _firstName = "";
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetField(ref _firstName, value))
                    OnPropertyChanged(nameof(FullName));
            }
        }

        private string _lastName = "";
        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetField(ref _lastName, value))
                    OnPropertyChanged(nameof(FullName));
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        private string _address = "";
        public string Address
        {
            get => _address;
            set => SetField(ref _address, value);
        }

        private DateTime _birthDate;
        public DateTime BirthDate
        {
            get => _birthDate;
            set => SetField(ref _birthDate, value);
        }

        private string _race = "";
        public string Race
        {
            get => _race;
            set => SetField(ref _race, value);
        }

        // ❗ DO NOT CHANGE GENDER LOGIC — per your instructions
        private Gender _gender = Gender.Unknown;
        public Gender Gender
        {
            get => _gender;
            set => SetField(ref _gender, value);
        }

        public List<MedicalNote> Notes { get; set; } = new();

        // ----------------------------
        // Helpers
        // ----------------------------
        protected bool SetField<T>(ref T backing, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(backing, value))
                return false;

            backing = value;
            OnPropertyChanged(name);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


    public enum Gender
    {
        Unknown,
        Male,
        Female,
        NonBinary,
        Other
    }
}
