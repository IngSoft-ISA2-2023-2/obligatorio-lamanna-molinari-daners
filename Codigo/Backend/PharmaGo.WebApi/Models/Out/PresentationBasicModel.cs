using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
    public class PresentationBasicModel
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public PresentationBasicModel(Presentation presentation)
        {
            Id = presentation.Id;
            Name = presentation.Name;
        }
    }
}
