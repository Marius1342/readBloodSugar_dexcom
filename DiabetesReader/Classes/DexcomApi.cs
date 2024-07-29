using ModernHttpClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesReader
{
    class DexcomApi
    {
        private string username = null;
        private string password = null;
        private string sessionId = null;

        public string SessionId = "";

        private const string EU_HOST = "shareous1.dexcom.com";
        private const string US_HOST = "share2.dexcom.com";
        //From github 
        private const string APPID = "d8665ade-9673-4e27-9ff6-92db4ce13d13";


        private static HttpClient client = new HttpClient(new NativeMessageHandler());

        public enum Language
        {
            EN,
            DE
        }


        public DexcomApi(string username, string password, string oldSessionId = null)
        {
            this.username = username;
            this.password = password;
            if (oldSessionId == null)
            {
                //Get token
                Task<bool> sucess = getSessionToken();
                sucess.Wait();
                if (sucess.Result)
                {
                    return;
                }
                else
                {
                    throw new Exception("Wrong User/Password");
                }
            }
            sessionId = oldSessionId;
            SessionId = sessionId;

        }





        private bool testSessionId(string id)
        {
            try
            {
                getValue().Wait();
            }
            catch
            {
                return false;
            }
            return true;


        }


        private async Task<string> getAccountId()
        {
            string accountUrl = GetApiUrl("General/AuthenticatePublisherAccount");
            JObject valuePairs = new JObject();
            valuePairs.Add("password", password);
            valuePairs.Add("accountName", username);
            valuePairs.Add("applicationId", APPID);
            string json = await PostData(accountUrl, valuePairs.ToString(Newtonsoft.Json.Formatting.None));
            //{ is only there when there is an error
            if (json.Contains("{"))
            {
                throw new Exception("Invalid password or username");
            }
            return json;


        }
        private async Task<bool> getSessionToken()
        {
            string id;
            try
            {
                id = await getAccountId();
            }
            catch
            {
                return false;
            }
            JObject json = new JObject();
            json.Add("applicationId", APPID);
            json.Add("accountId", id.Replace("\"", ""));
            json.Add("password", password);
            string getUrl = GetApiUrl("General/LoginPublisherAccountById");
            string re = await PostData(getUrl, json.ToString(Newtonsoft.Json.Formatting.None));

            sessionId = re.Replace("\"", "");

            if (sessionId.StartsWith("{"))
            {
                return false;
            }

            SessionId = sessionId;

            return true;
        }


        public async Task<string> getValue()
        {
            string data;
            try
            {

                string getUrl = GetApiUrl("Publisher/ReadPublisherLatestGlucoseValues");
                JObject json = new JObject
                {
                    { "maxCount", 1 },
                    { "minutes", 60 },
                    { "sessionId", sessionId }
                };


                data = await PostData(getUrl, json.ToString(Newtonsoft.Json.Formatting.None));
                if (data.StartsWith("[") == false)
                {
                    throw new Exception("New session token");

                }
            }
            catch
            {
                //Get new token
                await getSessionToken();
                string getUrl = GetApiUrl("Publisher/ReadPublisherLatestGlucoseValues");
                JObject json = new JObject();
                json.Add("maxCount", 1);
                json.Add("minutes", 60);
                json.Add("sessionId", sessionId);
                data = await PostData(getUrl, json.ToString(Newtonsoft.Json.Formatting.None));

            }

            return data;
        }
        private async Task<string> PostData(string url, string json)
        {
            var response = client.PostAsync(
            url,
             new StringContent(json, Encoding.UTF8, "application/json"));
            return await response.Result.Content.ReadAsStringAsync();
        }


        private string GetApiUrl(string resource)
        {
            if (Preferences.Get("US", false) == false)
            {
                return $"https://{EU_HOST}/ShareWebServices/Services/{resource}";
            }
            return $"https://{US_HOST}/ShareWebServices/Services/{resource}";
        }

        public static string ConvertSignToText(string sign, Language lang = Language.EN)
        {

            if (lang == Language.DE)
            {
                return ConvertSignGerman(sign);
            }
            return ConvertSignEnglish(sign);


        }


        private static string ConvertSignEnglish(string sign)
        {
            switch (sign)
            {
                case "DoubleUp":
                    return "double up";

                case "SingleUp":
                    return "just up";

                case "FortyFiveUp":
                    return "slowly up";

                case "Flat":
                    return "staying the same";

                case "FortyFiveDown":
                    return "slowly down";

                case "SingleDown":
                    return "just down";

                case "DoubleDown":
                    return "double down";

                default:
                    return sign;


            }

        }


        private static string ConvertSignGerman(string sign)
        {
            switch (sign)
            {
                case "DoubleUp":
                    return "doppel nach oben";

                case "SingleUp":
                    return "einfach nach oben";

                case "FortyFiveUp":
                    return "langsam nach oben";

                case "Flat":
                    return "gleich bleibend";

                case "FortyFiveDown":
                    return "langsam nach unten";

                case "SingleDown":
                    return "einfach nach unten";

                case "DoubleDown":
                    return "doppel nach unten";

                default:
                    return sign;


            }

        }


    }
}

