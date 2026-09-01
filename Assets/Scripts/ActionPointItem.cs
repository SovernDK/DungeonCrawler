using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionPointItem : MonoBehaviour
{
    public Image icon;
    public Sprite iconFull;
    public Sprite iconEmpty;

    public void Active()
    {
        icon.sprite = iconFull;
    }

    public void Unactive()
    {
        icon.sprite = iconEmpty;
    }
}
