using System.Collections.ObjectModel;
using System.Linq;
using GenshinInventory.Models;
using GenshinInventory.Services;
using Microsoft.Maui.Graphics;

namespace GenshinInventory.Views
{
    public partial class UsersPage : ContentPage
    {
        private DatabaseService db = new DatabaseService();
        private int editingProductId = 0;

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Log Out",
                "Are you sure you want to log out?",
                "Yes",
                "No");
            if (confirm)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        public ObservableCollection<ProductDisplay> Products { get; set; }

        public UsersPage()
        {
            InitializeComponent();

            Products = new ObservableCollection<ProductDisplay>();
            UsersCollectionView.ItemsSource = Products;

            LoadProducts();
        }

        private string GetStockStatus(int quantity)
        {
            if (quantity == 0)
                return "Out of Stock";
            else if (quantity <= 5)
                return "Low Stock";
            else
                return "In Stock";
        }

        private void LoadProducts()
        {
            Products.Clear();

            var productList = db.GetProducts();

            foreach (var product in productList)
            {
                Products.Add(new ProductDisplay
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Brand = product.Brand,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Status = GetStockStatus(product.Quantity), StatusColor = GetStatusColor(product.Quantity)
                });
            }
        }

        private void OnAddUserClicked(object sender, EventArgs e)
        {
            editingProductId = 0;

            PopupTitleLabel.Text = "Add Product";
            SaveButton.Text = "Add Product";

            ProductNameEntry.Text = string.Empty;
            CategoryEntry.Text = string.Empty;
            BrandEntry.Text = string.Empty;
            QuantityEntry.Text = string.Empty;
            PriceEntry.Text = string.Empty;

            PopupOverlay.IsVisible = true;

            ProductNameEntry.Focus();
        }

        private void OnProductNameCompleted(object sender, EventArgs e)
        {
            CategoryEntry.Focus();
        }

        private void OnCategoryCompleted(object sender, EventArgs e)
        {
            BrandEntry.Focus();
        }

        private void OnBrandCompleted(object sender, EventArgs e)
        {
            QuantityEntry.Focus();
        }

        private void OnQuantityCompleted(object sender, EventArgs e)
        {
            PriceEntry.Focus();
        }

        private void OnPriceCompleted(object sender, EventArgs e)
        {
            OnSaveUserClicked(sender, e);
        }

        private void OnClosePopup(object sender, EventArgs e)
        {
            PopupOverlay.IsVisible = false;
        }

        private async void OnSaveUserClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameEntry.Text) ||
                string.IsNullOrWhiteSpace(CategoryEntry.Text) ||
                string.IsNullOrWhiteSpace(BrandEntry.Text) ||
                string.IsNullOrWhiteSpace(QuantityEntry.Text) ||
                string.IsNullOrWhiteSpace(PriceEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }
            if (!int.TryParse(QuantityEntry.Text, out int quantity) ||
                !decimal.TryParse(PriceEntry.Text, out decimal price))
            {
                await DisplayAlert("Error", "Invalid quantity or price.", "OK");
                return;
            }
            try
            {
                Product product = new Product
                {
                    Id = editingProductId,
                    ProductName = ProductNameEntry.Text ?? "",
                    Category = CategoryEntry.Text ?? "",
                    Brand = BrandEntry.Text ?? "",
                    Quantity = int.Parse(QuantityEntry.Text ?? "0"),
                    Price = decimal.Parse(PriceEntry.Text ?? "0")
                };

                if (editingProductId == 0)
                {
                    db.AddProduct(product);
                    await DisplayAlert("Success", "Product added successfully.", "OK");
                }
                else
                {
                    db.UpdateProduct(product);
                    await DisplayAlert("Success", "Product updated successfully.", "OK");
                }

                ProductNameEntry.Text = string.Empty;
                CategoryEntry.Text = string.Empty;
                BrandEntry.Text = string.Empty;
                QuantityEntry.Text = string.Empty;
                PriceEntry.Text = string.Empty;

                PopupOverlay.IsVisible = false;
                editingProductId = 0;

                LoadProducts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void OnEditUserClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter != null)
            {
                int id = Convert.ToInt32(button.CommandParameter);

                var selectedProduct = Products.FirstOrDefault(p => p.Id == id);
                if (selectedProduct != null)
                {
                    editingProductId = selectedProduct.Id;

                    PopupTitleLabel.Text = "Edit Product";
                    SaveButton.Text = "Update Product";

                    ProductNameEntry.Text = selectedProduct.ProductName;
                    CategoryEntry.Text = selectedProduct.Category;
                    BrandEntry.Text = selectedProduct.Brand;
                    QuantityEntry.Text = selectedProduct.Quantity.ToString();
                    PriceEntry.Text = selectedProduct.Price.ToString();

                    PopupOverlay.IsVisible = true;
                }
            }
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter != null)
            {
                int id = Convert.ToInt32(button.CommandParameter);

                bool confirm = await DisplayAlert(
                    "Delete Product",
                    "Are you sure you want to delete this product?",
                    "Yes",
                    "No");

                if (confirm)
                {
                    db.DeleteProduct(id);
                    LoadProducts();

                    await DisplayAlert("Success", "Product deleted successfully.", "OK");
                }
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = e.NewTextValue?.Trim().ToLower() ?? "";

            Products.Clear();

            var productList = db.GetProducts();

            var filteredProducts = productList.Where(p =>
                p.ProductName.ToLower().Contains(keyword) ||
                p.Category.ToLower().Contains(keyword) ||
                p.Brand.ToLower().Contains(keyword));

            foreach (var product in filteredProducts)
            {
                Products.Add(new ProductDisplay
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Brand = product.Brand,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Status = GetStockStatus(product.Quantity), StatusColor = GetStatusColor(product.Quantity)
                });
            }
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }
        private Color GetStatusColor(int quantity)
        {
            if (quantity == 0)
                return Colors.Red;
            else if (quantity <= 5)
                return Colors.Orange;
            else
                return Colors.Green;
        }
    }

    public class ProductDisplay
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public Color StatusColor { get; set; } = Colors.White;
    }
}