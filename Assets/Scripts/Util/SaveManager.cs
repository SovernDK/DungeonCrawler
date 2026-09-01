using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public void Save()
    {
        SaveDataWrapper saveDataWrapper = new SaveDataWrapper();
        IEnumerable<ISaveableObject> saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveableObject>();
        foreach (ISaveableObject saveable in saveables)
        {
            saveDataWrapper.saveableObjects.Add(saveable.Save());
        }

        string filePath = Path.Combine(Application.persistentDataPath+"/Saves/", "save.json");
        string content = JsonUtility.ToJson(saveDataWrapper, true);
        
        Debug.Log("Content: " + content);

        File.WriteAllText(filePath, content);
    }

    public List<SaveableObject> Load()
    {
        string filePath = Path.Combine(Application.persistentDataPath+"/Saves/", "save.json");
        string json = File.ReadAllText(filePath);

        SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);

        return wrapper.saveableObjects;
    }
}

public class SaveDataWrapper
{
    public List<SaveableObject> saveableObjects = new List<SaveableObject>();

}