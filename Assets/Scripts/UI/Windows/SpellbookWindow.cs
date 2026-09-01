using System.Collections;
using System.Collections.Generic;
using AtlasGUI;
using UnityEngine;

public class SpellbookWindow : Window
{
    public Transform content;
    public SpellDescription spellDescription;    
    private SpellbookList list;

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        list = GetComponent<SpellbookList>();
        list.window = this;
        list.Initialize(context, content);
    }

    public override void Draw()
    {
        base.Draw();
    }
}
