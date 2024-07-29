using DiabetesReader.Classes;
using DiabetesReader;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace DiabetesReader.Pages;
public partial class Read : ContentPage
{
    private bool countdownStop = false;

    public Read()
    {
        InitializeComponent();

        Task.Run(ReadValue);
    }

    public async Task<bool> ReadValue()
    {

        if (Preferences.ContainsKey("setup") == false)
        {
            await Helper.Invoke(() => { message.Text = "Please setup your Account in the Settings"; });
            return true;
        }
        DexcomApi dexcom;

        //Check if all Preferences are set
        if (Preferences.ContainsKey("name") == false || Preferences.ContainsKey("password") == false)
        {

            await Helper.Invoke(() => { DisplayAlert("error", "Please configure the your account", "Ok"); });
            return true;
        }


        try
        {
            dexcom = new DexcomApi(Preferences.Get("name", ""), Preferences.Get("password", ""), Preferences.Get("token", null));
        }
        catch
        {
            await Helper.Invoke(() => { DisplayAlert("Error", "Invalid password or username", "OK"); });
            return true;

        }





        Task<string> task = dexcom.getValue();
        task.Wait();
        JArray array = JArray.Parse(task.Result);
        JObject json = JObject.Parse(array.First.ToString(Newtonsoft.Json.Formatting.None));
        string value = json.GetValue("Value").ToString();
        string sign = json.GetValue("Trend").ToString();

        //Set the value text to the value
        await Helper.Invoke(() => { message.Text = value; });


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

        if (Preferences.Get("readLoud", true))
        {
            if (language == DexcomApi.Language.EN)
            {
                try
                {
                    await TextToSpeech.Default.SpeakAsync("Blood sugar is " + value + " with the sign " + sign);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error with TextToSpeech.SpeakAsync: {ex.Message}");
                }
            }
            else
            {
                try
                {
                    await TextToSpeech.Default.SpeakAsync("Wert ist " + value + " mit dem Zeichen " + sign);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error with TextToSpeech.SpeakAsync: {ex.Message}");
                }

            }
        }
        else
        {
            Logger.Log("No speak, readLoud is false or not defined");
        }



        if (GlobalVars.autoClose)
        {
            GlobalVars.autoClose = false;
            GlobalVars.haveRead = true;
            await Helper.Invoke(() => { StartCountDown(); });

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



#if ANDROID
        //Android minimize the app
        var activity = Platform.CurrentActivity;
        activity.MoveTaskToBack(true);
#else
        //Only in windows close the app
        Application.Current.Quit();
#endif

    }
    public async void ShowStats(object sender, EventArgs e)
    {

        try
        {
            await ReadValue();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.StackTrace, "OK");
        }
    }
}