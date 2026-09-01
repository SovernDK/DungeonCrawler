using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtlasGUI;
using System.IO;
using System.Linq;

public class LoadMapWindow : Window
{
    public GameObject buttonPrefab;
    
    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);
        
        string filePath = Path.Combine(Application.persistentDataPath);
        if (!Directory.Exists(filePath))
        {
            Debug.LogError("Directory does not exist: " + filePath);
            return;
        }
    
        List<string> jsonMapFiles = Directory.GetFiles(filePath, "*.json")
            .Where(file => Path.GetFileNameWithoutExtension(file).EndsWith("_map"))
            .ToList();

        foreach (string file in jsonMapFiles)
        {    
            GameObject buttonClone = Instantiate(buttonPrefab, transform);
            buttonClone.GetComponent<ButtonString>().Initialize(Path.GetFileName(file), file);
            buttonClone.GetComponent<ButtonString>().Connect(Load);
            Debug.Log("Found JSON Map File: " + file);
        }
    }

    public override void Draw()
    {
        base.Draw();
    }

    public void Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
        }

        string json = File.ReadAllText(filePath);
        (context as MainContext).grid.Deserialize(json);
        (context as MainContext).grid.UpdateGrid();
        Debug.Log("Loaded map from "+filePath);
    }
}
