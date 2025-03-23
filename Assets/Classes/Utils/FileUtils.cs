using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class FileUtils
{
    #nullable enable
    public static List<string> ListJSONFiles(string directory)
    {
        string[] array = Directory.GetFiles(PrependAssetsDir(directory), "*.json", SearchOption.AllDirectories);

        return new List<string>(array);
    }

    public static string RemoveJSONExtension(string fullPath)
    {
        string str = Path.GetFileName(fullPath);

        return str[..^5];
    }

    public static T ReadJsonFromFile<T>(string root, string filePath) where T : class
    {
        string fullPath = PrependAssetsDir("/" + root + "/" + filePath + ".json");
        string jsonString = File.ReadAllText(fullPath);

        return JsonUtility.FromJson<T>(jsonString);
    }

    public static T ReadJSON<T>(string fullPath) where T : class
    {
        return JsonUtility.FromJson<T>(File.ReadAllText(fullPath));
    }
    #nullable disable

    private static string PrependAssetsDir(string path)
    {
        return Path.Combine(Application.dataPath + "/" + path);
    }
}
