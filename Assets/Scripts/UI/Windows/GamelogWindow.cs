using System.Collections.Generic;
using AtlasGUI;
using TMPro;
using UnityEngine;

public class GamelogWindow : Window
{
    public GameObject entryPrefab;
    public Transform content;

    private List<GameObject> gameObjects;

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);
        gameObjects = new List<GameObject>();

        Gamemaster.Instance.gamelog.Logs.ForEach(log => 
        {
            gameObjects.Add(Instantiate(entryPrefab, content));
        });

        Gamemaster.Instance.gamelog.onAddEntry += Draw;
    }

    public override void Draw()
    {
        base.Draw();

        gameObjects.ForEach(gameObject => { Destroy(gameObject); });

        Gamemaster.Instance.gamelog.Logs.ForEach(log => 
        {
            GameObject entryGo = Instantiate(entryPrefab, content);
            entryGo.GetComponentInChildren<TMP_Text>().text = log.log;
            gameObjects.Add(entryGo);
        });
    }
}
