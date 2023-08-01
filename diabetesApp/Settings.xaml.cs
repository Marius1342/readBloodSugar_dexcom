namespace diabetesApp;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
        ReadSettings();
    }

    public void ReadSettings()
    {
        if (Preferences.ContainsKey("setup") == false)
        {
            return;
        }

        Passwort.Text = Preferences.Get("password", "");
        Name.Text = Preferences.Get("name", "");
        autoRead.IsChecked = Preferences.Get("autoRead", false);
        US.IsChecked = Preferences.Get("US", false);
        int english = Preferences.Get("lang", 0);
        if (english == 0)
        {
            english_radio.IsChecked = true;
        }
        if (english == 1)
        {
            german_radio.IsChecked = true;
        }
    }



    public void btnShowPassword(object sender, EventArgs e)
    {
        //Hide or Show password, inverse
        Passwort.IsPassword = !Passwort.IsPassword;
        //Update text
        showPassword.Text = Passwort.IsPassword ? "Show" : "Hide";
    }

    private async void Save(object sender, EventArgs e)
    {
        Preferences.Set("password", Passwort.Text);
        Preferences.Set("name", Name.Text);
        Preferences.Set("autoRead", autoRead.IsChecked);
        //Reverse
        Preferences.Set("lang", english_radio.IsChecked == true ? 0 : 1);

        Preferences.Set("US", US.IsChecked);

        Preferences.Set("setup", "1");


        await DisplayAlert("Saved", "Settings are saved", "OK");
    }


}