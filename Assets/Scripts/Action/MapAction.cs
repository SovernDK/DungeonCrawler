using Crawler.Actors;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Crawler;

[CreateAssetMenu(fileName = "MapAction", menuName = "Actions/MapAction")]
public class MapAction : Action
{
    [SerializeField]
    private Texture2D map;

    [SerializeField]
    private List<EffectPrototype> effects;
    public List<EffectPrototype> Effects { get => effects; set => effects = value; }

    public override void Execute(Actor user, Actor other = null)
    {
        (Gamemaster.Instance.context as MainContext).mainScene.ToggleWindow("Map");
        MapWindow window = (Gamemaster.Instance.context as MainContext).mainScene.GetWindow("Map") as MapWindow;
        if(window == null)
        {
            Debug.LogError("Invalid");
        }
        window.Map.texture = map;
    }

    private bool HasEffectsScript(GameObject go)
    {
        return go != null && go.GetComponent<ParticleFx>() != null;
    }
}