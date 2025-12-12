using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Infrastructure
{
    public class MyJsonSerializer
    {
        public const string UNITS_PATH = "units.json";
        public const string GAMES_PATH = "games.json";
        static public void writeToJson<T>(T obj,string path)
        {
            string str=JsonSerializer.Serialize(obj);
            File.WriteAllText(path, str);
        }
        static public T readFromJson<T>(string path)
        {
            string str = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(str);
        }
    }
}
