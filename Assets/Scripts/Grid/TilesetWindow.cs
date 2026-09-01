using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtlasGUI;
using TMPro;
using Crawler;
using Crawler.MapEditor;

public class TilesetWindow : Window
{
    public TMP_Dropdown dropdownGroup;
    public TMP_Dropdown dropdownType;
    public GameObject tileTypeButton, commandButton;
    public Transform commandContent;

    private MapEditorContext mapEditorContext;

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        mapEditorContext = context as MapEditorContext;

        dropdownGroup.options.Clear();
        dropdownType.options.Clear();

        foreach (TileGroup tileGroup in System.Enum.GetValues(typeof(TileGroup)))
        {
            string optionName = $"{tileGroup}";
            dropdownGroup.options.Add(new TMP_Dropdown.OptionData(optionName));
        }
        dropdownGroup.onValueChanged.AddListener(delegate { mapEditorContext.mapEditor.ChangeGroup(dropdownGroup.value); });

        foreach (TileType tileType in System.Enum.GetValues(typeof(TileType)))
        {
            string optionName = $"{tileType}";
            dropdownType.options.Add(new TMP_Dropdown.OptionData(optionName));
        }

        dropdownType.onValueChanged.AddListener(mapEditorContext.mapEditor.ChangeType);
        dropdownType.onValueChanged.AddListener(mapEditorContext.mapEditor.pointer.GetComponent<MapEditorPointer>().ChoosePrototype);

        int index = 0;
        foreach(MapEditorCommand command in mapEditorContext.mapEditor.Commands)
        {
            string optionName = command.Name;
            GameObject go = Instantiate(commandButton, commandContent);
            go.GetComponent<ButtonInt>().Initialize(optionName, index);
            go.GetComponent<ButtonInt>().Connect(mapEditorContext.mapEditor.SetCommand);

            index++;
        }
    }

    public override void Draw()
    {
        base.Draw();
    }
}
