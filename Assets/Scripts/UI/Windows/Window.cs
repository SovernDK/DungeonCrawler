using UnityEngine;
using UnityEngine.EventSystems;

namespace AtlasGUI 
{
    public abstract class Window : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] 
        private string windowName;

        [SerializeField] 
        private bool alwaysOpen;

        [SerializeField] 
        private bool draggable;
        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector2 offset;
        protected CoreContext context;

        public string WindowName { get => windowName; set => windowName = value; }
        public bool AlwaysOpen { get => alwaysOpen; set => alwaysOpen = value; }


        public virtual void Initialize(CoreContext context)
        {
            this.context = context;
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        public virtual void Draw() 
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(draggable)
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(draggable)
            {
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out position);
                rectTransform.localPosition = position - offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Optionally add code to handle when dragging ends
        }
    }
}