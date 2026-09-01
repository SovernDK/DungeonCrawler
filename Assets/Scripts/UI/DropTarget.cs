using UnityEngine;
using UnityEngine.EventSystems;

public class DropTarget : MonoBehaviour, IDropHandler
{
    protected Draggable draggable;
    public virtual void OnDrop(PointerEventData eventData)
    {
        draggable = eventData.pointerDrag.GetComponent<Draggable>();
    }
}
