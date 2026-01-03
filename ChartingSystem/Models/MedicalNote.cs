namespace MedicalCharting.Models
{
    public class MedicalNote
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string Diagnosis { get; set; } = "";
        public List<string> Prescriptions { get; set; } = new();

        public override string ToString() =>
            $"{Date:yyyy-MM-dd}: {Diagnosis} | Rx: {string.Join(", ", Prescriptions)}";
    }
}