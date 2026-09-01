using Crawler.Actors;
using Sheet;
using UnityEngine;
using System.Collections.Generic;
using Crawler;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "UiAction", menuName = "Actions/UiAction")]
public class UiAction : Action
{
    [SerializeField] [BoxGroup("General")]
    private EUiActionType type;

    [SerializeField] [BoxGroup("ToogleWindow", VisibleIf = "@type == EUiActionType.ToogleWindow")]
    private string windowName;

    [SerializeField]
    private List<EffectPrototype> effects;

    public EUiActionType Type { get => type; set => type = value; }
    public string WindowName { get => windowName; set => windowName = value; }
    public List<EffectPrototype> Effects { get => effects; set => effects = value; }

    public override void Execute(Actor user, Actor other = null)
    {
        if (type == EUiActionType.ToogleWindow)
        {
            (Gamemaster.Instance.context as MainContext).mainScene.ToggleWindow(windowName);
        }

        effects.ForEach(f => 
            Gamemaster.Instance.effectsManager.AddEffect(f.gameObject.name, user.transform.position + user.DamageEffectAnchor, Quaternion.identity, f.delay)
        );
    }

    private bool HasEffectsScript(GameObject go)
    {
        return go != null && go.GetComponent<ParticleFx>() != null;
    }
}

public enum EUiActionType 
{
    ToogleWindow
}