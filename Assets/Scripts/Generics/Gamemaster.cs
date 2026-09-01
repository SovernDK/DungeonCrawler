using AtlasGUI;
using UnityEngine.SceneManagement;
using Crawler;
using Crawler.Actors;

public class Gamemaster : Singleton<Gamemaster>
{
    public CoreContext context;
    public GamePreferences gamePreferences;
    public SaveManager saveManager;
    public EffectsManager effectsManager;
    public InputManager inputManager;
    public Gamelog gamelog;

    public Actor emptyActor;

    public bool initialized = false;
    public bool isGameLoaded = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void Load()
    {
        isGameLoaded = true;
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1" && !isGameLoaded)
        {
            LoadMap("start_map.json");
        }
        else if (scene.name == "Level1" && isGameLoaded)
        {
            LoadGame("start_map.json", "save");
        }
    }
    public void LoadMap(string map)
    {
        if(!initialized)
        {
            gamelog.Initialize();

            (context as MainContext).grid = FindObjectOfType<Crawler.Grid>();
            (context as MainContext).mainScene = FindObjectOfType<MainScene>();

            (context as MainContext).Initialize();
            gamePreferences.Initialize();
            
            (context as MainContext).grid.BuildMap(map);

            (context as MainContext).player = FindObjectOfType<Player>();
            (context as MainContext).player.Initialize(context as MainContext);
            (context as MainContext).mainScene.Initialize(context as MainContext);
            (context as MainContext).gameFlow.Initialize(context as MainContext);

            initialized = true;
        }
        else
        {
            gamelog.Initialize();
            PoolManager.Instance.ReturnAll();
            (context as MainContext).grid.BuildMap(map);
        }
    }

    public void LoadGame(string map, string saveFilePath)
    {
        if(!initialized)
        {
            gamelog.Initialize();
            (context as MainContext).grid = FindObjectOfType<Crawler.Grid>();
            (context as MainContext).mainScene = FindObjectOfType<MainScene>();

            (context as MainContext).Initialize();
            gamePreferences.Initialize();
            
            (context as MainContext).grid.BuildMap(map, saveManager.Load());

            (context as MainContext).player = FindObjectOfType<Player>();
            (context as MainContext).player.Initialize(context as MainContext);
            (context as MainContext).mainScene.Initialize(context as MainContext);
            (context as MainContext).gameFlow.Initialize(context as MainContext);

            initialized = true;
        }
        else
        {
            gamelog.Initialize();
            PoolManager.Instance.ReturnAll();
            (context as MainContext).grid.BuildMap(map, saveManager.Load());
        }
    }

    public void GameOver()
    {
        (context as MainContext).grid = null;
        (context as MainContext).mainScene = null;
        (context as MainContext).player = null;

        initialized = false;
        PoolManager.Instance.ReturnAll();

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}