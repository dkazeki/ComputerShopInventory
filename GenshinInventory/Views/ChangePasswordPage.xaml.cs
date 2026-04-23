namespace GenshinInventory.Views;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage()
	{
		InitializeComponent();
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

    private async void OnUpdatePasswordClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CurrentPasswordEntry.Text) ||
            string.IsNullOrWhiteSpace(NewPasswordEntry.Text) ||
            string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        if (NewPasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            await DisplayAlert("Error", "New passwords do not match.", "OK");
            return;
        }

        await DisplayAlert("Success", "Password updated successfully.", "OK");
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}