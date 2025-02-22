using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadKeys : MonoBehaviour
{
    private List<string> apiKeys = new List<string>();
    private int currentKeyIndex = 0;

    void Start()
    {
        LoadOpenAIKeys();
    }

    void LoadOpenAIKeys()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "config.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var config = JsonUtility.FromJson<ConfigWrapper>(json);
            foreach (var keyConfig in config.apiKeys)
            {
                apiKeys.Add(keyConfig.key);
            }
        }
    }

    public string GetNextKey()
    {
        var key = apiKeys[currentKeyIndex];
        currentKeyIndex = (currentKeyIndex + 1) % apiKeys.Count;
        return key;
    }
}

[System.Serializable]
public class ConfigWrapper
{
    public List<APIKeyConfig> apiKeys;
}

[System.Serializable]
public class APIKeyConfig
{
    public string key;
}
