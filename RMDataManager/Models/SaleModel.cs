using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMDataManager.Models
{
    public class SaleModel
    {
        // It's been initialized from the other side and not here in order to enable null-checking.
        public List<SaleDetailModel> SaleDetails { get; set; }
    }
}