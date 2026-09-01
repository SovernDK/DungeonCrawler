using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crawler.Commands 
{
    public class PlayerCommand
    {
        private string name;

        public string Name { get => name; set => name = value; }

        public PlayerCommand(string name)
        {
            this.name = name;
        }

        public virtual void Execute(MainContext context)
        {
            
        }
    }
}