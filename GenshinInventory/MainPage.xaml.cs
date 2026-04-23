using GenshinInventory.Services;

namespace GenshinInventory
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnTestConnectionClicked(object sender, EventArgs e)
        {
            try
            {
                var dbService = new DatabaseService();
                using var connection = dbService.GetConnection();
                await connection.OpenAsync();

                await DisplayAlert("Success", "Connected to MySQL successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}