using Crawler.Actors;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGridCondition", menuName = "AI/GridCondition")]
public class GridCondition : Condition
{
    [SerializeField] private EGridConditionType type;
    [SerializeField] private float distance;

    public EGridConditionType Type { get => type; set => type = value; }
    public float Distance { get => distance; set => distance = value; }

    public override bool Check(Actor actor)
    {
        if(type == EGridConditionType.TargetInDistance)
        {
            return Vector3Int.Distance(actor.CurrentTile.Tile.Coordinates, actor.Target.CurrentTile.Tile.Coordinates) <= distance;
        }
        
        return false;
    }
}

public enum EGridConditionType 
{
    TargetInDistance
}