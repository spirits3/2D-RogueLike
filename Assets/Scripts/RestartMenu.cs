using UnityEngine;

public class RestartMenu : MonoBehaviour
{

    public GameObject RestartMenuUi;

    private GameManager gameManager;
    public static Player instance;
    private bool restartMenu = true;

    private int food;

    public void Start()
    {
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    private void Update()
    {
        food = Player.instance.food;
        if (food <= 0 && restartMenu)
        {
            RestartMenuUi.SetActive(true);
            restartMenu = false;
        }
    }

    public void RestartGame()
    {
        gameManager.RestartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
