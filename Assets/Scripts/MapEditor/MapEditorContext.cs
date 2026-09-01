using Crawler;
using Crawler.MapEditor;
using UnityEngine;
using UnityEngine.Events;

public class MapEditorContext : CoreContext
{
    public MapEditorContextEvent onInit;

    public Database database;
    public Crawler.Grid grid;
    public Player player;
    public MapEditor mapEditor;

    private void Start()
    {
        grid.Initialize(database);
        onInit.Invoke(this);
    }
}

[System.Serializable]
public class MapEditorContextEvent : UnityEvent<MapEditorContext>
{
}
