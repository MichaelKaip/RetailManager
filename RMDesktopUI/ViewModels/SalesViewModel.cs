using System.ComponentModel;
using Caliburn.Micro;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        /*
         * Private backing fields
         */
        private BindingList<string> _products;
        private int _itemQuantity;
        private BindingList<string> _cart;

        /*
         * Public Properties
         */
        public BindingList<string> Products 
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
