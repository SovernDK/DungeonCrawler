using System.Collections;
using Crawler.Actors;
using UnityEngine;

namespace Crawler.Commands
{
    public class ActorCommand
    {
        private string name;
        protected Actor actor;
        protected MainContext context;

        public string Name { get => name; set => name = value; }

        public ActorCommand(MainContext context, Actor actor, string name)
        {
            this.name = name;
            this.actor = actor;
            this.context = context;
        }

        public virtual IEnumerator Execute()
        {
            yield return new WaitForEndOfFrame();
        }
    }
}