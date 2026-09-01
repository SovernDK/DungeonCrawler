using Sirenix;
using Sirenix.OdinInspector;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] [TabGroup("Interactable")] protected Color highlightColor = Color.red;
    [SerializeField] [TabGroup("Interactable")] protected Color originalColor;
    [SerializeField] [TabGroup("Interactable")] protected Renderer model;
    
    protected bool loaded;

    private void OnEnable() 
    {
        OnInit();
    }

    public virtual void OnInit() 
    { 
        // originalColor = model.material.color;
    }

    public virtual void OnLoaded() 
    { 
        loaded = true;
    }

    public virtual void OnEnter() 
    { 
        // model.material.color = highlightColor;    
    }

    public virtual void OnExit() 
    { 
        // model.material.color = originalColor;    
    }

    public virtual void OnInteractedWith(Player player) { }
}
