using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    public virtual void OnInit() {}

    public virtual void OnUpdate() {}

    private void Start() 
    {
        OnInit();
    }

    private void Update() 
    {
        OnUpdate();
    }
}