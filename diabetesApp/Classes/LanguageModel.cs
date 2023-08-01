
using System.IO;
using System.Text;
using Windows.ApplicationModel.UserDataTasks;

namespace diabetesApp.Classes
{
    public class LanguageModel
    {

        public enum Language
        {
            English = 0,
            German
        }

        private static readonly string[] allLanguages = { "English.txt", "German.txt" } ;
        private static bool init = false;
        private static Language _language = Language.German;
        private static List<string[]> models = new List<string[]>();
        public LanguageModel() {
             
            
        }

        public static void setLanguage(Language language)
        {
            _language = language;
        }

       public static async Task initModels()
       {

            models = new List<string[]>(allLanguages.Length);
            foreach (string language in allLanguages)
            {
                models.Add(new string[0]);
            }

            //Read files
            foreach (string fileName in allLanguages)
            {

                //Check if exist
                if (await FileSystem.Current.AppPackageFileExistsAsync(fileName) == false)
                {
                    Console.WriteLine("Error");
                    continue;
                }


                Stream data = await FileSystem.Current.OpenAppPackageFileAsync(fileName);

                byte[] bytes = new byte[data.Length];

                data.Read(bytes, 0, bytes.Length);

                string content = Encoding.UTF8.GetString(bytes);

                string[] lines = content.Split(Environment.NewLine.ToCharArray());

                //First line is the language
                bool success = int.TryParse(lines[0], out int res);

                if (success == false)
                {
                    Console.WriteLine("Error");
                    continue;
                }

                List<string> lines_ = new List<string>();

                //Remove all empty lines "", and skip the first
                for (int i = 1; i < lines.Length; i++)
                {
                    if (lines[i] == "")
                    {
                        continue;
                    }

                    lines_.Add(lines[i]);
                }


                models[res] = lines_.ToArray();


            }
            init = true;

        }


        public static string getContent(int id) {
            if (init == false)
            {
                initModels().Wait();
            }
            return models[(int)_language].ElementAt(id);
        }


    }
}
