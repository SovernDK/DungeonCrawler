using System.Collections;
using System.Collections.Generic;
using Crawler;
using Crawler.Actors;
using Crawler.Commands;
using UnityEngine;

public class ActorMoveTowardCommand : ActorCommand
{
    public ActorMoveTowardCommand(MainContext context, Actor actor, string name) : base(context, actor, name)
    {
    }

    public override IEnumerator Execute()
    {
        Vector3Int start = actor.CurrentTile.Tile.Coordinates;
        Vector3Int goal = actor.Target.CurrentTile.Tile.Coordinates;

        Queue<Tile> path = context.grid.Pathfinding.CreatePath(start, goal);

        Tile next = path.Dequeue();
        if(next.Coordinates == start && path.Count > 1) 
        {
            next = path.Dequeue();
        }
        else if(next.Coordinates == goal) 
        {
            yield return actor.ActorContext.PlayMoveBlocked();
        }

        Vector3Int direction = actor.CurrentTile.Tile.Coordinates - next.Coordinates;
        Vector3Int currentCoordinates = actor.CurrentTile.Tile.Coordinates;

        TileController tile = context.grid.MoveActorToTile(currentCoordinates, next.Coordinates);
        if(tile != null) 
        {
            actor.CurrentTile.Tile.IsOccupied = false;
            tile.Tile.IsOccupied = true;
            actor.CurrentTile = tile;
            actor.ActionPoints--;

            yield return actor.ActorContext.PlayMove(tile);
            tile.ExecuteActions(actor);
            if(tile.Trap) tile.Trap.Activate(actor);
        }
        else 
        {
            actor.ActionPoints--;
            yield return actor.ActorContext.PlayMoveBlocked();
        }
    }
}
