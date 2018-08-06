using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour {

    private GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
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
