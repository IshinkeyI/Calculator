using System.IO;

namespace Calculator
{
    internal class HistoryParser
    {

        private static string fullPath = Path.GetFullPath(@"HistoryInformation.txt");
        public static void GetAllInformation(ref Form1 form)
        {
            StreamReader reader = new StreamReader(fullPath, true);
            string[] historyElements = reader.ReadToEnd().Split('\n');
            foreach (string element in historyElements)
            {
                if(element == string.Empty)
                    continue;
                form.AddELementInHistory(element);
            }
            reader.Close();
        }

        public static bool SetNewInformation(string historyElement, string answer)
        {
            StreamReader reader = new StreamReader(fullPath, true);
            string[] historyElements = reader.ReadToEnd().Split('\n');
            foreach (string element in historyElements)
            {
                if (historyElement == element)
                {
                    reader.Close();
                    return false;
                }
            }
            reader.Close();

            StreamWriter writer = new StreamWriter(fullPath, true);
            writer.WriteLine(historyElement +'=' + answer);
            writer.Close();
            return true;
        }

        public static void ClearAllInformation()
        {
            File.WriteAllText(fullPath, "");
        }

    }
}
