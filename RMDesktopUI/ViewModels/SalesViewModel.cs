using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Models;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        /*
         * Private backing fields
         */
        private BindingList<ProductDisplayModel> _products;
        private int _itemQuantity = 1;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private readonly IProductEndPoint _productEndPoint;
        private ProductDisplayModel _selectedProduct;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndPoint _saleEndpoint;
        private CartItemDisplayModel _selectedCartItem;
        private IMapper _mapper;

        /*
         * Public Properties
         */
        public BindingList<ProductDisplayModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public ProductDisplayModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => _selectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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

        public string SubTotal => CalculateSubTotal().ToString("C");

        private decimal CalculateSubTotal()
        {
            var subTotal = Cart
                .Sum(item => (item.Product.RetailPrice * item.QuantityInCart));

            return subTotal;
        }

        public string Tax => CalculateTax().ToString("C");

        private decimal CalculateTax()
        {
            var taxRate = (_configHelper.GetTaxRate()/100);

            var taxAmount = Cart
                .Where(item => item.Product.IsTaxable)
                .Sum(item => (item.Product.RetailPrice * item.QuantityInCart * taxRate));

            return taxAmount;
        }

        public string Total
        {
            get
            {
                var total = CalculateSubTotal() + CalculateTax();

                return total.ToString("C");
            }
        }


        public BindingList<CartItemDisplayModel> Cart
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
        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, 
            ISaleEndPoint saleEndpoint, IMapper mapper)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;

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
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
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
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            // Either updating the quantity of an existing item in cart
            // or adding a new item.
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                var item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };

                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                // Make sure something is selected
                var output = SelectedCartItem != null && SelectedCartItem?.Product.QuantityInStock > 0;

                return output;
            }
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut
        {
            get
            {
                // Make sure there is something in the cart
                var output = Cart.Count > 0;

                return output;
            }
        }

        public async Task CheckOut()
        {
            // Create a SaleModel 
            SaleModel sale = new SaleModel();

            // Converts the cart from the CartItemDisplayModel list over to a SaleModel with an
            // internal List ofa SaleDetailModel which just holds the ProductId and Quantity
            // from the Frontend.
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            //Post the SaleDetailModel to the API
            await _saleEndpoint.PostSale(sale);

        }


    }
}