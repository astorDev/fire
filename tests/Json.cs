using System.Text.Json;

public static class Json
{
    public static T DeserializeFile<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions.Web)!;
    }
}