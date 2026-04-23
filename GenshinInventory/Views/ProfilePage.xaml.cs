using GenshinInventory.Services;

namespace GenshinInventory.Views
{
    public partial class ProfilePage : ContentPage
    {
        private DatabaseService db = new DatabaseService();

        public ProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private void LoadProfile()
        {
            var profile = db.GetAdminProfile();
            FullNameLabel.Text = profile.FullName;
            UsernameLabel.Text = "@" + profile.Username;
        }

        private async void OnStocksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UsersPage());
        }

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

        private async void OnEditProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfilePage());
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }
    }
}