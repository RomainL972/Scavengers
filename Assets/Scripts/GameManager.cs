/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int StartPlayerFoodPoints = 100;
    private const int StartLevel = 0;

    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public BoardManager boardScript;
    public int playerFoodPoints = StartPlayerFoodPoints;
    [HideInInspector] public bool playersTurn = true;
    public GameObject mainCamera;

    //Store a reference to our BoardManager which will set up the level.
    //private DeviceChange deviceChange;
    private Text levelText;
    private GameObject levelImage;
    private int level = StartLevel;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;
    private bool gameOver = false;

    //Awake is always called before any Start functions
    void Awake()
    {
    	//Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
            
    	    //If instance already exists and it's not this:
        else if (instance != this)
                
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    
            
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
            
        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //deviceChange = GetComponent<DeviceChange>();
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        level++;
        InitGame();
    }

    /*void OnOrientationChange()
    {
        switch(Input.deviceOrientation)
        {
            case DeviceOrientation.Portrait:
            case DeviceOrientation.PortraitUpsideDown:
                Camera.main.orthographicSize = 10;
                break;
            default:
                Camera.main.orthographicSize = 5;
                break;
        }
    }*/

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene(level);

        //deviceChange.OnOrientationChange.AddListener(OnOrientationChange);
        //OnOrientationChange();
#if UNITY_ANDROID && !UNITY_EDITOR
        Camera.main.orthographicSize = 10;
#else
        Camera.main.orthographicSize = 5;
#endif
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.\n\nPress enter or touch screen\nto restart";
        levelImage.SetActive(true);
        gameOver = true;
    }

    void Update() {
        if (gameOver)
        {
            foreach(char c in Input.inputString)
            {
                if(c == '\n' || c == '\r')
                {
                    gameOver = false;
                    Restart();
                    return;
                }
            }
            if(Input.touchCount > 0)
            {
                gameOver = false;
                Restart();
            }
        }

        if (playersTurn || enemiesMoving || doingSetup || gameOver)
            return;
        StartCoroutine(MoveEnemies());
    }

    private void Restart()
    {
        level = StartLevel;
        playerFoodPoints = StartPlayerFoodPoints;
        SoundManager.instance.musicSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public bool IsDoingSetup()
    {
        return doingSetup;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}