using System.ComponentModel;
using System.Linq;
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
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private IProductEndPoint _productEndPoint;
        private ProductModel _selectedProduct;

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

        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
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
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                var subTotal = Cart.Sum(item => (item.Product.RetailPrice * item.QuantityInCart));

                return subTotal.ToString("C");
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


        public BindingList<CartItemModel> Cart
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
                // Make sure something is selected and an item quantity is given.
                var output = ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;

                return output;
            }
        }

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            // Either updating the quantity of an existing item in cart
            // or adding a new item.
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;

                // Not the best solution - it's just tricking the system to update the list.
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                var item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };

                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
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

            NotifyOfPropertyChange(() => SubTotal);
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
