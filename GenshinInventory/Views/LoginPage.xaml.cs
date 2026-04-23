using GenshinInventory.Services;
using MySqlConnector;

namespace GenshinInventory.Views
{
    public partial class LoginPage : ContentPage
    {
        private DatabaseService db = new DatabaseService();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim() ?? "";
            string password = PasswordEntry.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter username and password.", "OK");
                return;
            }

            try
            {
                bool isValid = db.ValidateAdmin(username, password);

                if (isValid)
                {
                    await Navigation.PushAsync(new UsersPage());
                }
                else
                {
                    await DisplayAlert("Error", "Invalid admin login.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void OnUsernameCompleted(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }
        private void OnPasswordCompleted(object sender, EventArgs e)
        {
            OnLoginClicked(sender, e);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UsernameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
        }
        private void OnLoginEnterPressed(object sender, EventArgs e) 
        {
            OnLoginClicked(sender, e);
        }
    }
}