using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private GameInputs gameInputs;
    private InputActionMap actionMap;

    private Vector2 movement;
    private float rotation;

    private bool back;

    public Vector2 Movement { get => movement; set => movement = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public bool Back { get => back; set => back = value; }
    public GameInputs GameInputs { get => gameInputs; set => gameInputs = value; }

    void Awake()
    {
        gameInputs = new GameInputs();
        actionMap = gameInputs.PlayerControl;

        LoadBindings();

        gameInputs.PlayerControl.Movement.performed += value => { movement = value.ReadValue<Vector2>(); };
        gameInputs.PlayerControl.Rotation.performed += value => { rotation = value.ReadValue<float>(); };

        // gameInputs.PlayerControl.Confirm.performed += ctx => { confirm = ctx.ReadValue<float>(); Debug.Log("LBM: "+ctx.ReadValue<float>()); };
        
        gameInputs.PlayerControl.Back.performed += _ => { back = true; };
        gameInputs.PlayerControl.Back.canceled += _ => { back = false; };
    }

    void OnEnable()
    {
        actionMap.Enable();
    }

    void OnDisable()
    {
        actionMap.Disable();
    }

    public List<(string ActionName, string BindingName, string BindingPath)> GetAllBindings()
    {
        List<(string ActionName, string BindingName, string BindingPath)> bindings = new List<(string ActionName, string BindingName, string BindingPath)>();
        foreach (var action in actionMap)
        {
            foreach (var binding in action.bindings)
            {
                if (binding.isComposite)
                {
                    string compositeName = binding.name;
                    continue;
                }
                else if (binding.isPartOfComposite)
                {
                    bindings.Add((action.name, binding.name, binding.effectivePath));
                }
                else if (!string.IsNullOrEmpty(binding.effectivePath))
                {
                    bindings.Add((action.name, binding.name, binding.effectivePath));
                }
            }
        }
        return bindings;
    }


    public void RebindAction(string actionName, string newKey)
    {
        var action = actionMap.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError("Action named " + actionName + " not found.");
            return;
        }

        action.ChangeBinding(0).WithPath(newKey);
        SaveBindings();
    }

    public void SaveBindings()
    {
        foreach (var action in actionMap)
        {
            PlayerPrefs.SetString(action.name + "_binding", action.bindings[0].effectivePath);
        }
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        foreach (var action in actionMap)
        {
            var binding = PlayerPrefs.GetString(action.name + "_binding", string.Empty);
            if (!string.IsNullOrEmpty(binding))
            {
                action.ChangeBinding(0).WithPath(binding);
            }
        }
    }

    public void ResetToDefaultBindings()
    {
        foreach (var action in actionMap)
        {
            var defaultBinding = action.bindings[0].path; // Assuming the original binding is the default
            action.ChangeBinding(0).WithPath(defaultBinding);
        }
        SaveBindings(); // Optionally save after resetting to ensure PlayerPrefs are updated
    }
}
