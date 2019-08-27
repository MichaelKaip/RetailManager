using System.ComponentModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        /*
         * Private backing fields
         */
        private BindingList<ProductModel> _products;
        private int _itemQuantity;
        private BindingList<string> _cart;
        private IProductEndPoint _productEndPoint;

        /*
         * Public Properties
         */
        public BindingList<ProductModel> Products 
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }


        public int ItemQuantity // Can be int even though it's a text box in the form.
                                // That enables error checking in Caliburn Micro
                                // and it's not possible to type in text.
        {
            get => _itemQuantity;
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public string SubTotal
        {
            get
            {
                // Todo: Replace with calculation
                return "$0.00";
            }
        }

        public string Tax
        {
            get
            {
                // Todo: Replace with calculation
                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                // Todo: Replace with calculation
                return "$0.00";
            }
        }


        public BindingList<string> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        /*
         * Constructors
         */
        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndPoint = productEndPoint;
        }

        /*
         * Methods
         */
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndPoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected and an item quantity is given.

                return output;
            }
        }

        public void AddToCart()
        {

        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected

                return output;
            }
        }

        public void RemoveFromCart()
        {

        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                // Make sure there is something in the cart

                return output;
            }
        }

        public void CheckOut()
        {

        }


    }
}
