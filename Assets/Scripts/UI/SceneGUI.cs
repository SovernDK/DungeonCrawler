using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor.GettingStarted;

namespace AtlasGUI 
{
    public abstract class SceneGUI : MonoBehaviour
    {
        protected CoreContext context;
        protected Dictionary<string, Window> windows;

        public virtual void Initialize(CoreContext context)
        {
            this.context = context;

            LoadAndInitializeSceneWindows(context);
        }
        
        public virtual void Reinitialize(string name)
        {
            Window window;
            if (windows.TryGetValue(name, out window))
            {
                window.Initialize(context);
            }
        }

        public virtual void OpenWindow(string name)
        {
            Window window;
            if (windows.TryGetValue(name, out window))
            {
                window.gameObject.SetActive(true);
                window.Draw();
            }
        }

        public virtual void CloseWindow(string name)
        {
            Window window;
            if (windows.TryGetValue(name, out window))
            {
                window.gameObject.SetActive(false);
            }
        }

        public virtual void ToggleWindow(string name)
        {
            Window window;
            if (windows.TryGetValue(name, out window))
            {
                window.gameObject.SetActive(!window.gameObject.activeSelf);
            }
        }

        public virtual bool IsWindowOpen(string name)
        {
            Window window;
            if (windows.TryGetValue(name, out window))
            {
                return window.gameObject.activeSelf;
            }
            return false;
        }

        public virtual void CloseAllWindows()
        {
            windows.Values.ForEach(window => {
                if(!window.AlwaysOpen) window.gameObject.SetActive(false);
            });
        }

        public virtual Window GetWindow(string name)
        {
            Window window;
            if (windows.TryGetValue(name, out window))
            {
                return window;
            }

            Debug.LogError("Window by name " + name + " not found");
            return null;
        }

        protected virtual void LoadAndInitializeSceneWindows(CoreContext context)
        {
            windows = new Dictionary<string, Window>();
            Window[] windowsInScene = FindObjectsOfType<Window>(true);

            foreach (Window window in windowsInScene)
            {
                window.Initialize(context);
                windows.Add(window.WindowName, window);
            }
        }
    }
}