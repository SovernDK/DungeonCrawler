using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HandVisualHandler : MonoBehaviour
{
    public List<HandVisual> visuals;

    public void HideAll()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }   

    public void Show(int id)
    {
        HandVisual v = visuals.Find(vis => vis.itemCard.Item.Id == id);
        if(v != null)
        {
            v.visual.SetActive(true);
        }
    }

    public void Show(Item.EWeaponType type)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.GetChild((int) type).gameObject.SetActive(true);
    }   
}

[Serializable]
public class HandVisual
{
    [HorizontalGroup("General")] [HideLabel]
    public ItemCard itemCard;

    [HorizontalGroup("General")] [HideLabel]
    public GameObject visual;
}
