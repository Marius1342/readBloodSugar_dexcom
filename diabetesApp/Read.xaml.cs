
using diabetisApp;
using Newtonsoft.Json.Linq;


namespace diabetesApp;
public partial class Read : ContentPage
{
    private bool countdownStop = false;

    public Read()
    {
        InitializeComponent();

        ReadValue();
    }

    public async Task<bool> ReadValue()
    {
        if (Preferences.ContainsKey("setup") == false)
        {
            message.Text = "Please setup your Account in the Settings";
            return true;
        }
        DexcomApi dexcom;

        //Check if all Preferences are set
        if (Preferences.ContainsKey("name") == false || Preferences.ContainsKey("password") == false)
        {
            await DisplayAlert("error", "Please configure the your account", "Ok");
            return true;
        }


        try
        {
            dexcom = new DexcomApi(Preferences.Get("name", ""), Preferences.Get("password", ""), Preferences.Get("token", null));
        }
        catch
        {
            await DisplayAlert("Error", "Invalid password or username", "OK");
            return true;

        }



        Task<string> task = dexcom.getValue();
        task.Wait();
        JArray array = JArray.Parse(task.Result);
        JObject json = JObject.Parse(array.First.ToString(Newtonsoft.Json.Formatting.None));
        string value = json.GetValue("Value").ToString();
        string sign = json.GetValue("Trend").ToString();


        DexcomApi.Language language;

        //Check if language is set
        if (Preferences.ContainsKey("lang") == false)
        {
            language = DexcomApi.Language.DE;
        }
        else
        {
            language = (DexcomApi.Language)Preferences.Get("lang", ((int)DexcomApi.Language.EN));
        }

        sign = DexcomApi.ConvertSignToText(sign, language);


        if (language == DexcomApi.Language.EN)
        {
            await TextToSpeech.SpeakAsync("Blood sugar is " + value + " with the sign " + sign);
        }
        else
        {
            await TextToSpeech.SpeakAsync("Wert ist " + value + " mit dem Zeichen " + sign);

        }



        if (GlobalVars.autoClose)
        {
            GlobalVars.autoClose = false;
            GlobalVars.haveRead = true;
            StartCountDown();
        }

        return true;
    }


    private void StopCountDownButton(object sender, EventArgs e)
    {
        countdownStop = true;
    }
    private async void StartCountDown()
    {
        int countdown = 0;
        // wait 11 seconds
        while (countdown < 11 && countdownStop == false)
        {
            await Task.Delay(1000);
            countdownLabel.Text = (11 - countdown).ToString();
            countdown++;
        }


        if (countdownStop == true)
        {
            return;
        }

        //Auto close
        GlobalVars.haveRead = false;
        Application.Current.Quit();
    }
    public async void ShowStats(object sender, EventArgs e)
    {

        try
        {
            ReadValue();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.StackTrace, "OK");
        }
    }
}