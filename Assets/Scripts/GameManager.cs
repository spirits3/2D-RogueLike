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

    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private static bool savedLevel = false;
    private Text levelText;
    private GameObject levelImage;
    private GameObject restartMenu;
    private GameObject pauseMenu;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetUp;
    private GameObject board;
    private Player player;

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
        levelImage = GameObject.Find("LevelImage");
    }

    private void OnLevelWasLoaded(int index)    
    {
        if (SceneManager.GetActiveScene().name != "Menu" &&
            SceneManager.GetActiveScene().name != "Options" &&
            !PauseMenu.GameIsPaused)
        {
            level++;
        }

        if(!PauseMenu.GameIsPaused)
            InitGame();
    }

    void InitGame()
    {
        doingSetUp = true;
        levelImage = GameObject.Find("LevelImage");
        restartMenu = GameObject.Find("RestartMenu");
        pauseMenu = GameObject.Find("PauseMenu");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetUpScene(level);
    }

    void SaveMap()
    {
        board = GameObject.Find("Board");
        DontDestroyOnLoad(board);
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
    }

    public void RestartGame()
    {
        level = 0;
        Player.instance.food = 100;
        SceneManager.LoadScene("Game");
        SoundManager.instance.musicSource.Play();
    }

    // Update is called once per frame
    void Update () {

        if (PauseMenu.GameIsPaused && !savedLevel)
        {
            SaveMap();
            savedLevel = true;
        }

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