using PharmaGo.Domain.Enums;

namespace PharmaGo.Domain.Entities
{
    public class Presentation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
    }
}