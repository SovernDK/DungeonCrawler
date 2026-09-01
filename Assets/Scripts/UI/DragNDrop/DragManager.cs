using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform dragLayer = null;

    public RectTransform DragLayer { get => dragLayer; set => dragLayer = value; }
}