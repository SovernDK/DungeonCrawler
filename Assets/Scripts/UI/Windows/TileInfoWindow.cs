using AtlasGUI;
using Crawler;
using Crawler.MapEditor;
using TMPro;
using System;

public class TileInfoWindow : Window
{
    public TMP_Text label, coordinates;
    public TMP_Dropdown features;

    private TileController tile;
    private MapEditorContext mapEditorContext;

    public TileController Tile { get => tile; set => tile = value; }


    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        mapEditorContext = context as MapEditorContext;
        
        if(tile != null) 
        {
            label.text = tile.gameObject.name;
            coordinates.text = tile.Tile.Coordinates.ToString();
        }

        features.options.Clear();
        foreach (ETileFeature tileFeature in Enum.GetValues(typeof(ETileFeature)))
        {
            string optionName = $"{tileFeature}";
            features.options.Add(new TMP_Dropdown.OptionData(optionName));
        }
        features.onValueChanged.AddListener(mapEditorContext.mapEditor.DrawFeature);
    }

    public override void Draw()
    {
        base.Draw();
        
        if(tile != null) 
        {
            label.text = tile.gameObject.name;
            coordinates.text = tile.Tile.Coordinates.ToString();
            features.SetValueWithoutNotify((int) tile.Tile.Feature);
        }
    }
}
