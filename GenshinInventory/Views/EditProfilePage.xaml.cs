using GenshinInventory.Models;
using GenshinInventory.Services;

namespace GenshinInventory.Views
{
    public partial class EditProfilePage : ContentPage
    {
        private DatabaseService db = new DatabaseService();
        private AdminProfile currentProfile = new AdminProfile();

        public EditProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private void LoadProfile()
        {
            currentProfile = db.GetAdminProfile();

            FullNameEntry.Text = currentProfile.FullName;
            UsernameEntry.Text = currentProfile.Username;
            EmailEntry.Text = currentProfile.Email;
        }

        private async void OnStocksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UsersPage());
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            try
            {
                currentProfile.FullName = FullNameEntry.Text ?? "";
                currentProfile.Username = UsernameEntry.Text ?? "";
                currentProfile.Email = EmailEntry.Text ?? "";

                db.UpdateAdminProfile(currentProfile);

                await DisplayAlert("Success", "Profile updated successfully.", "OK");
                await Navigation.PushAsync(new ProfilePage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}