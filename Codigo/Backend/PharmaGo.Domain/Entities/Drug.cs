using PharmaGo.Exceptions;

namespace PharmaGo.Domain.Entities
{
    public class Drug
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symptom { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; } = 0;
        public bool Prescription { get; set; }
        public bool Deleted { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public Presentation Presentation { get; set; }
        public Pharmacy? Pharmacy { get; set; }

        public void ValidOrFail()
        {
            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Symptom) 
                    || Quantity <= 0 || Price <= 0 || Stock < 0 
                    || UnitMeasure == null || Presentation == null || Pharmacy == null)
            {
                throw new InvalidResourceException("The Drug is not correctly created.");
            }
        }
    }
}