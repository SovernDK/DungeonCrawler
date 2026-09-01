using Crawler.Actors;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCondition", menuName = "AI/Condition")]
public class Condition : ScriptableObject
{
    public virtual bool Check(Actor actor)
    {
        return true;
    }
}
