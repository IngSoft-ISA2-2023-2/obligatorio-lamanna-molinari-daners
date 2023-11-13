using PharmaGo.Domain.Entities;
using PharmaGo.WebApi.Models.In;
using static PharmaGo.WebApi.Models.In.PurchaseModelRequest;

namespace PharmaGo.WebApi.Converters
{
    public class PurchaseModelRequestToPurchaseConverter
    {

        public Purchase Convert(PurchaseModelRequest model)
        {

            var purchase = new Purchase();
            purchase.PurchaseDate = model.PurchaseDate;
            purchase.BuyerEmail = model.BuyerEmail;
            purchase.details = new List<PurchaseDetail>();
            purchase.products = new List<PurchaseDetailProduct>();
            foreach (var detail in model.Details)
            {
                purchase.details
                    .Add(new PurchaseDetail
                    {
                        Quantity = detail.Quantity,
                        Drug = new Drug { Code = detail.Code },
                        Pharmacy = new()
                        {
                            Id = detail.PharmacyId
                        }
                    });
            }
            foreach (var purchaseDetailProduct in model.DetailsProducts)
            {
                purchase.products.Add(new PurchaseDetailProduct
                {
                    Pharmacy = new Pharmacy { Id = purchaseDetailProduct.PharmacyId },
                    Quantity = purchaseDetailProduct.Quantity,
                    Product = new Product { Code = purchaseDetailProduct.Code}
                });
            }
            return purchase;
        }

    }
}
