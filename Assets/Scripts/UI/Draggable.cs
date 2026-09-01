using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 originalPosition;
    protected Transform draggableObject;
    protected RectTransform draggableParent;

    [SerializeField] 
    protected RectTransform dragLayer;

    public Transform DraggableObject { get => draggableObject; set => draggableObject = value; }

    public virtual void Initialize()
    {
        draggableParent = transform.GetComponent<RectTransform>();
        dragLayer = transform.parent.GetComponent<DragManager>().DragLayer;
        // if(GetComponentInParent<DragManager>() != null)
        // {
        //     dragLayer = GetComponentInParent<DragManager>().DragLayer;
        // }
        // else
        // {
            
        // }
        originalPosition = transform.position;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(draggableObject != null) 
        {
            draggableObject.SetParent(dragLayer);
        }
    }
  
    public virtual void OnDrag(PointerEventData eventData)
    {
        draggableObject.transform.Translate(eventData.delta);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        draggableObject.SetParent(draggableParent);
        draggableObject.localPosition = Vector2.zero;
    }
}
