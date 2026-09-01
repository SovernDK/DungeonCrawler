using AtlasGUI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sheet;
using Crawler.Actors;

public class CharacterSheetWindow : Window
{
    [SerializeField] private DerivedAttributeSlider hp, mp, st;

    public DerivedAttributeSlider Hp { get => hp; set => hp = value; }

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        CharacterSheet characterSheet = (context as MainContext).player.Actor.CharacterSheet;

        hp.Sheet = characterSheet;
        hp.Initialize();

        mp.Sheet = characterSheet;
        mp.Initialize();

        st.Sheet = characterSheet;
        st.Initialize();

        (context as MainContext).player.Actor.CharacterSheet.onAttributeValueChange += Draw;
    }

    public override void Draw()
    {
        base.Draw();
        hp.Draw();
        mp.Draw();
    }
}
