using UnityEngine;
using System;

[Serializable]
public class SaveableObject
{
    public string name;
    public string objectId;
    public string type;
    public Vector3 position;
    public Quaternion rotation;
    public string data; // JSON string of the specific object's data
}
