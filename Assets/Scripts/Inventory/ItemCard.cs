using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Data/Item", order = 1)]
public class ItemCard : ScriptableObject
{
    [SerializeField] [InlineProperty] [HideLabel] private Item item;
    public Item Item { get => item; set => item = value; }
}
