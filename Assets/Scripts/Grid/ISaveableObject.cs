using UnityEngine;

public interface ISaveableObject
{
    string GetObjectId();

    SaveableObject Save();
    void Load(SaveableObject saveData);
}
