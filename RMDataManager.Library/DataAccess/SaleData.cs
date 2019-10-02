using System;
using System.Collections.Generic;
using System.Linq;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using RMDesktopUI.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId) 
        {
            // Todo: Make this S.O.L.I.D!!!

            // 1.) Create the models to be saved to the database and
            // fill in the available information.
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = (ConfigHelper.GetTaxRate()/100);

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                // Get the information about the products from the database
                var productInfo = products.GetProductById(item.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductId} could not be found in the database");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                details.Add(detail);
            }

            // 3.) Create the sale model
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("RMDatabase");

                    // 4. Save the sale model to the database
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    // 5.) Getting the sale id
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>(
                        "spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                    // 6.) Finish filling in the SaleDetailModel and Save the sale detail models
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        // In cases where the database is going to be called thousands of times,
                        // it's possible to use "table value parameters" here instead and save the whole table.
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }
        }

        public List<SaleReportModel> GetSaleReport()
        {
              var sql = new SqlDataAccess();

              var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", 
                  new { }, "RMDatabase");

              return output;
        }
    }
}
