using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button mainMenu, load, settings, exit;

    private void Start() 
    {
        mainMenu.onClick.AddListener(Gamemaster.Instance.NewGame);
    }
}
