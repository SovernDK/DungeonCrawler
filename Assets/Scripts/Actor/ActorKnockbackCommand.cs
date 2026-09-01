using System.Collections;
using Crawler.Actors;
using UnityEngine;

namespace Crawler.Commands
{
    public class ActorKnockbackCommand : ActorCommand
    {
        private Vector3Int direction;
        public ActorKnockbackCommand(MainContext context, Actor actor, string name, Vector3Int direction) : base(context, actor, name)
        {
            this.direction = direction;
        }

        public override IEnumerator Execute()
        {
            Vector3Int currentCoordinates = actor.CurrentTile.Tile.Coordinates;
            Vector3Int target = currentCoordinates + direction;
            
            TileController tile = context.grid.MoveActorToTile(currentCoordinates, target);
            if(tile != null) 
            {
                actor.CurrentTile.Tile.IsOccupied = false;
                tile.Tile.IsOccupied = true;
                actor.CurrentTile = tile;

                yield return actor.ActorContext.PlayKnockback(tile);
                
                tile.ExecuteActions(actor);
                if(tile.Trap) tile.Trap.Activate(actor);
            }
            else 
            {
                yield return actor.ActorContext.PlayMoveBlocked();
            }
        }
    }
}