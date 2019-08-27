using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    // Follows the same pattern as in UserData.cs
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            // new{} just creates an anonymous object which, in this case,
            // is just empty because we don't need this parameter here.
            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll",
                new{}, "RMDatabase");

            return output;
        }
    }
}
