using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtlasGUI;
using TMPro;
using UnityEngine.UI;

public class SaveMapWindow : Window
{
    public TMP_InputField input;
    public Button button;

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        button.onClick.AddListener(Save);
    }

    public void Save()
    {
        string filePath = Path.Combine(Application.persistentDataPath, input.text+"_map.json");
        string content = (context as MapEditorContext).grid.Serialize();

        File.WriteAllText(filePath, content);
        Debug.Log("Map Saved to " + filePath);
    }

    public override void Draw()
    {
        base.Draw();
    }
}
