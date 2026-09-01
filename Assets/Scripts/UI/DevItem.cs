using UnityEngine;
using UnityEngine.UI;

public class DevItem : MonoBehaviour, IAtlasListItem
{
    public Image icon;

    private Item item;

    public Item Item { get => item; set => item = value; }

    public void Initialize()
    {
        icon.sprite = item.Icon;
    }

    public void Draw()
    {
        icon.sprite = item.Icon;
    }
}
