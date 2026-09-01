using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorPointer : MonoBehaviour
{
    public List<GameObject> prototype;
    public void ChoosePrototype(int id)
    {
        foreach(GameObject go in prototype)
        {
            go.SetActive(false);
        }
        
        prototype[id].SetActive(true);
    }

    public void MovePointer(Vector3 newPosition)
    {
        transform.position = newPosition;
        
        if(transform.position.x < 0 || transform.position.z < 0)
        {
            foreach(GameObject go in prototype)
            {
                go.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
        else
        {
            foreach(GameObject go in prototype)
            {
                go.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
    }
}