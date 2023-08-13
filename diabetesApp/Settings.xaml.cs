using diabetesApp.Classes;
using diabetisApp;

namespace diabetesApp;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
        ReadSettings();

        if(Logger.LogCount > 0)
        {
            //errorEmailSend.Text = LanguageModel.getContent(1);
            errorEmailSend.Text = "Send error report";
            errorEmailSend.IsEnabled = true;
        }
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

        //SMS
        checkSms.IsChecked = Preferences.Get("checkSms", false);
        telnumber.Text = Preferences.Get("telnumber", "");
        smsContent.Text = Preferences.Get("smsContent", "");

        //Only android
#if ANDROID
    askPermissions();
#endif
    }


    private async void askPermissions()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Sms>();
        if(status == PermissionStatus.Granted)
        {
            return;
        }

        //Ask for permission
        status = await Permissions.RequestAsync<Permissions.Sms>();
        if(status == PermissionStatus.Granted)
        {
            return;
        }

        await DisplayAlert("Warning", "The listen function for the sms does not work", "Ok");

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
        //SMS 
        Preferences.Set("checkSms", checkSms.IsChecked);
        Preferences.Set("telnumber", telnumber.Text);
        Preferences.Set("smsContent", smsContent.Text);

        Preferences.Set("password", Passwort.Text);
        Preferences.Set("name", Name.Text);
        Preferences.Set("autoRead", autoRead.IsChecked);
        //Reverse
        Preferences.Set("lang", english_radio.IsChecked == true ? 0 : 1);

        Preferences.Set("US", US.IsChecked);

        Preferences.Set("setup", "1");


        await DisplayAlert("Saved", "Settings are saved", "OK");
    }

    private async void errorEmailSend_Clicked(object sender, EventArgs e)
    {
        if (Email.Default.IsComposeSupported)
        {

            string subject = "Found some bug";
            string body = "I found some bugs" + Environment.NewLine;

            body += Logger.GetReportForEmail();


            string[] recipients = new[] {GlobalVars.DEV_EMAIL};

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(recipients)
            };

            await Email.Default.ComposeAsync(message);
        }
    }
}