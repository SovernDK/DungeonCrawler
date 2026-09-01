using UnityEngine.Events;
using Crawler;
using AtlasGUI;
using UnityEngine;

public class MainContext : CoreContext
{
    public MainContextEvent onInit;

    public Player player;
    public Database database;
    public Config config;
    public Crawler.Grid grid;
    public MainScene mainScene;

    public GameFlow gameFlow;

    public void Initialize()
    {
        grid.Initialize(database);
    }
}

[System.Serializable]
public class MainContextEvent : UnityEvent<MainContext>
{
}
