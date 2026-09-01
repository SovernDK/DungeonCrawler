using AtlasGUI;
using UnityEngine.UI;
using UnityEngine;

public class MapWindow : Window
{
    [SerializeField]
    private RawImage map;

    public RawImage Map { get => map; set => map = value; }
}
