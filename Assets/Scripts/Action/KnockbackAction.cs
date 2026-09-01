using Crawler.Actors;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockbackActionAction", menuName = "Actions/KnockbackActionAction")]
public class KnockbackAction : Action
{
    [SerializeField]
    private int strength;

    public override void Execute(Actor user, Actor other)
    {
        base.Execute(user, other);

        Vector3Int current = user.CurrentTile.Tile.Coordinates;
        Vector3Int target = other.CurrentTile.Tile.Coordinates;
        Vector3Int direction = target - current;

        other.ActorContext.Knockback(strength, direction);
    }
}