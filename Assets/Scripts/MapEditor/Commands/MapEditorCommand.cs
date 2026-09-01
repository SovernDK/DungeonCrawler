using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crawler.MapEditor {
    public abstract class MapEditorCommand
    {
        private string name;

        public string Name { get => name; set => name = value; }

        public MapEditorCommand(string name)
        {
            this.name = name;
        }

        public virtual void Execute(MapEditor mapEditor)
        {
            
        }
    }
}