using Newtonsoft.Json;
using System;
using System.IO;

namespace CompetitionTask.Utils
{
    public class JsonUtils
    {
        private static readonly string BaseFilePath = @"D:\Mansi-Industryconnect\CompetitionTask\JsonData\";

        public static List<T> ReadJsonData<T>(string jsonDataFile)
        {
            string jsonFilePath = Path.Combine(BaseFilePath, jsonDataFile);
            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException($"File not found: {jsonFilePath}");
            }
            string jsonData = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<List<T>>(jsonData);
        }   
    }
}
