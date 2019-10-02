using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using RMDataManager.Models;
using SaleModel = RMDesktopUI.Library.Models.SaleModel;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
           //Handle the ata which are received in the API
           var data = new SaleData();
           var cashierId = RequestContext.Principal.Identity.GetUserId();

           data.SaveSale(sale, cashierId);
        }

        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            var data = new SaleData();
            return data.GetSaleReport();
        }
    }
}
