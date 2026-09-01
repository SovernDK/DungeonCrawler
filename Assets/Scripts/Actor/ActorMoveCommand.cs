using System.Collections;
using Crawler.Actors;
using UnityEngine;

namespace Crawler.Commands
{
    public class ActorMoveCommand : ActorCommand
    {
        private Vector3Int direction;
        public ActorMoveCommand(MainContext context, Actor actor, string name, Vector3Int direction) : base(context, actor, name)
        {
            this.direction = direction;
        }

        public override IEnumerator Execute()
        {
            Vector3Int currentCoordinates = actor.CurrentTile.Tile.Coordinates;
            Vector3 localDirection = actor.transform.TransformDirection(direction);
            Vector3Int targetDirection = new Vector3Int(Mathf.RoundToInt(localDirection.x), 0, Mathf.RoundToInt(localDirection.z));
            Vector3Int target = actor.CurrentTile.Tile.Coordinates + targetDirection;

            TileController tile = context.grid.MoveActorToTile(currentCoordinates, target);
            if(tile != null) 
            {
                actor.CurrentTile.Tile.IsOccupied = false;
                tile.Tile.IsOccupied = true;
                actor.CurrentTile = tile;
                actor.ActionPoints = Mathf.Clamp(actor.ActionPoints - 1, 0, actor.MaxActionPoints);

                yield return actor.ActorContext.PlayMove(tile);

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