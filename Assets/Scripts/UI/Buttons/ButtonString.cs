using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace AtlasGUI 
{
    public class ButtonString : MonoBehaviour
    {
        public TMP_Text label;
        public string value;
        public UnityEvent<string> OnValueCalled = new UnityEvent<string>();

        public void Initialize(string label, string value)
        {
            this.label.text = label;
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

        public void Connect(UnityAction<string> action)
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