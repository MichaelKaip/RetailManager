using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace RMDesktopUI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        public ProductDisplayModel Product { get; set; }

        private int _quantityInCart;

        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        public int QuantityInCart
        {
            get => _quantityInCart;
            set
            {
                _quantityInCart = value;
                OnPropertyChanged(nameof(QuantityInCart));
                OnPropertyChanged(nameof(DisplayText));
            }
        }


        public string DisplayText => $"{Product.ProductName} ({QuantityInCart})";

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
