using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
    public class PharmacyDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<UserBasicModel> Users { get; set; }
        public ICollection<DrugBasicModel> Drugs { get; set; }


        public PharmacyDetailModel(Pharmacy pharmacy)
        {
            Id = pharmacy.Id;
            Name = pharmacy.Name;
            Address = pharmacy.Address;
            Users = new List<UserBasicModel>();
            foreach (var user in pharmacy.Users)
            {
                Users.Add(new UserBasicModel(user));
            }
            Drugs = new List<DrugBasicModel>();
            foreach (var drug in pharmacy.Drugs)
            {
                Drugs.Add(new DrugBasicModel(drug));
            }
        }
    }
}
