using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace AtlasGUI 
{
    public class ButtonInt : MonoBehaviour
    {
        public TMP_Text label;
        public int value;
        public UnityEvent<int> OnValueCalled = new UnityEvent<int>();

        public void Initialize(string label, int value)
        {
            if(this.label != null)
            {
                this.label.text = label;
            }
            this.value = value;

            Button button = GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(() => OnValueCalled.Invoke(value));
            }
            else
            {
                Debug.LogError("Button component not found on the GameObject.");
            }
        }

        public void Connect(UnityAction<int> action)
        {
            if (action != null)
            {
                OnValueCalled.AddListener(action);
            }
            else
            {
                Debug.LogError("The action to connect is null.");
            }
        }
    }
}