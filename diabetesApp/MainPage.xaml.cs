using diabetisApp;
using System.Net.Sockets;
using System.Net;
using diabetesApp.Classes;
//using Android.Content;

namespace diabetesApp;

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        InitializeComponent();

        version.Text ="Version: " +  GlobalVars.VERSION;


        CheckNewVersion();


        //Auto read
        if (Preferences.ContainsKey("setup") && Preferences.Get("autoRead", false))
        {
            autoRead();

            //Check for update
            


        }


    }

    private async void CheckNewVersion()
    {


        Logger.Log("Check for new version");
        if (await Updater.NewVersion())
        {
            Logger.Log("New version found");
            updateButton.IsVisible = true;
        }
        else
        {
            Logger.Log("No new version");
            updateButton.IsVisible = false;
        }
    }

    async void autoRead()
    {
        if (GlobalVars.haveRead)
        {
            return;
        }
        Guid oldId = Application.Current.MainPage.Id;
        await Task.Delay(2000);


        //Checks if user is using this future
        if (Application.Current.MainPage.Id != oldId) {
            //User is active
            return;
        }


        GlobalVars.autoClose = true;
        Read(this, null);

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopToRootAsync();
        await Application.Current.MainPage.Navigation.PushAsync(new Settings());
    }
    private async void Read(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopToRootAsync();
        await Application.Current.MainPage.Navigation.PushAsync(new Read());
    }

    private void updateButton_Clicked(object sender, EventArgs e)
    {
        Updater.openDownloadSite();
    }
}

