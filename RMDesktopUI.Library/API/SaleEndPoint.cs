using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.API
{
    public class SaleEndPoint : ISaleEndPoint
    {
        public IAPIHelper _apiHelper;

        public SaleEndPoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/sale", sale)) 
            {
                if (response.IsSuccessStatusCode)
                {
                    // Todo: Log successful call (???)
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
