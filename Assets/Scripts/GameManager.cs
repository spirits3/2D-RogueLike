using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    public BoardManager boardScript;    

   
    //a changer après les tests
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private GameObject restartMenu;
    private GameObject pauseMenu;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetUp;

	// Use this for initialization
	void Awake () {
        if(!instance)
        {
            instance = this; 
        }else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
    }

    private void Start()
    {
        if(PauseMenu.GameIsPaused)
        {
            pauseMenu.SetActive(true);
        }
    }

    private void OnLevelWasLoaded(int index)    
    {
        if (SceneManager.GetActiveScene().name != "Menu" &&
            SceneManager.GetActiveScene().name != "Options" &&
            PauseMenu.GameIsPaused == false)
        {
            level++;
        }

            InitGame();

        if (level != 0)
        {
            restartMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }
    }


    void InitGame()
    {
        doingSetUp = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        restartMenu = GameObject.Find("RestartMenu");
        pauseMenu = GameObject.Find("PauseMenu");
        levelText.text = "Day " + level;
        restartMenu.SetActive(false);
        pauseMenu.SetActive(false);
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetUpScene(level);

    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetUp = false;
    }
	
    public void GameOver()
    {
        levelImage.SetActive(true);
        levelText.text = "After " + level + " days, you starved.";
        restartMenu.SetActive(true);
    }

    public void RestartGame()
    {
        level = 0;
        Player.instance.food = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.instance.musicSource.Play();
    }

    // Update is called once per frame
    void Update () {
        Debug.Log("gamePause: " + PauseMenu.GameIsPaused);
        if (playersTurn || enemiesMoving || doingSetUp)
            return;
        StartCoroutine(MoveEnemies());  
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0) {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
