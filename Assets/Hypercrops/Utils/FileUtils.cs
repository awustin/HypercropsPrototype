using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;

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

    public static T? ReadAssetJSON<T>(string assetsRelativePath) where T : class
    {
        try
        {
        string fullPath = PrependAssetsDir(assetsRelativePath);
        string jsonString = File.ReadAllText(fullPath);

        return JsonConvert.DeserializeObject<T>(jsonString);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error reading or deserializing JSON from {assetsRelativePath}: {ex.Message}");
            throw;
        }
    }

    public static T ReadJSON<T>(string fullPath) where T : class
    {
        return JsonUtility.FromJson<T>(File.ReadAllText(fullPath));
    }
    #nullable disable

    private static string PrependAssetsDir(string path)
    {
        return Path.Combine($"{Application.dataPath}/{(path.StartsWith('/') ? path[1..] : path)}");
    }
}
