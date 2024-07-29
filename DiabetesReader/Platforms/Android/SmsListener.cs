using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Newtonsoft.Json.Linq;
using DiabetesReader.Classes;

namespace DiabetesReader.Platforms.Android
{
    [Service(Enabled = true, Exported = true)]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" }, Priority = (int)IntentFilterPriority.HighPriority)]

    internal class SmsListener : Service
    {
        private const string SmsIntentAction = "android.provider.Telephony.SMS_RECEIVED";

        private SmsBroadcastReceiver smsBroadcastReceiver;

        public override IBinder OnBind(Intent intent) => null;

        public override void OnCreate()
        {
            base.OnCreate();
            smsBroadcastReceiver = new SmsBroadcastReceiver();

        }
        public override void OnStart(Intent intent, int startId)
        {
            base.OnStart(intent, startId);
            RegisterSmsReceiver();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterSmsReceiver();
        }

        private void RegisterSmsReceiver()
        {
            var filter = new IntentFilter(SmsIntentAction);
            RegisterReceiver(smsBroadcastReceiver, filter);
        }

        private void UnregisterSmsReceiver()
        {
            UnregisterReceiver(smsBroadcastReceiver);
        }
    }
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class SmsBroadcastReceiver : BroadcastReceiver
    {
        public override async void OnReceive(Context context, Intent intent)
        {
            if (Preferences.Get("checkSms", false) == false)
            {
                return;
            }


            if (intent.Action == "android.provider.Telephony.SMS_RECEIVED")
            {
                var smsMessages = Telephony.Sms.Intents.GetMessagesFromIntent(intent);
                if (smsMessages != null && smsMessages.Length > 0)
                {
                    foreach (var smsMessage in smsMessages)
                    {

                        string senderPhoneNumber = smsMessage.OriginatingAddress;
                        string text = smsMessage.MessageBody;




                        //Check if sms is from the configured sender
                        if (Preferences.Get("telnumber", "") != senderPhoneNumber)
                        {
                            continue;
                        }
                        //Check if sms content is the same as configured
                        if (Preferences.Get("smsContent", "") != text)
                        {
                            continue;
                        }


                        //Open read 
                        DexcomApi dexcom;

                        //Check if all Preferences are set
                        if (Preferences.ContainsKey("name") == false)
                        {
                            return;
                        }


                        try
                        {
                            dexcom = new DexcomApi(Preferences.Get("name", ""), Preferences.Get("password", ""), Preferences.Get("token", null));
                        }
                        catch
                        {
                            Logger.Error("Error while creating the DexcomApi in the service");
                            return;

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


                        //Task is needed or the APP crashes 

                        if (language == DexcomApi.Language.EN)
                        {
                            await Task.Run(async () =>
                            {
                                TextToSpeech.SpeakAsync("Blood sugar is " + value + " with the sign " + sign).Wait();
                            });
                        }
                        else
                        {
                            await Task.Run(async () =>
                            {
                                TextToSpeech.SpeakAsync("Wert ist " + value + " mit dem Zeichen " + sign).Wait();

                            });
                        }



                    }
                }

            }
        }
    }
}
