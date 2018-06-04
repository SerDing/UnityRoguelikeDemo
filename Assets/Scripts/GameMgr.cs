using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour {

    public int level = 0;
    public int foodNum = 6;
    public float levelStartDelay = 2f;
    public List<Enemy> enemyList = new List<Enemy>();
    
    [HideInInspector]public bool isEnd = false;
    private bool skipStep = true;
    private bool doingSetup;
    public bool enemyAI = false;

    private Text levelText;
    private Text foodText;
    private GameObject levelImage;
    private Hero hero;
    private SceneMgr sceneMgr;
    

    public static GameMgr _instance = null;
    public static GameMgr Instance{
        get{
            return _instance;
        }
    }

    public AudioClip gameOverSound;

    void Awake () {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded; // 在委托中添加方法 OnSceneLoaded()

        InitGame();
	}
	
    void InitGame()
    {
        doingSetup = true;
        isEnd = false;
        enemyList.Clear();

        sceneMgr = GetComponent<SceneMgr>();
        sceneMgr.InitScene();

        levelImage = GameObject.Find("levelImage");
        levelText = GameObject.Find("levelText").GetComponent<Text>();
        foodText = GameObject.Find("FoodText").GetComponent<Text>();

        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        
        Invoke("HideLevelImage", levelStartDelay);
        UpdateFoodText(0);

        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();

    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    void UpdateFoodText(int foodChange)
    {
        if(foodChange == 0)
        {
            foodText.text = "Food:" + foodNum;
        }
        else
        {
            string str = "";

            if(foodChange < 0)
            {
                str = foodChange.ToString();
            }
            else
            {
                str = "+" + foodChange.ToString();
            }

            foodText.text = str + " Food:" + foodNum;
        }
        
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void ReduceFood(int num)
    {
        foodNum -= num;

        UpdateFoodText(-num);

        if(foodNum <= 0)
        {
            GameOver();
        }
    }

    public void AddFood(int num)
    {
        foodNum += num;

        UpdateFoodText(num);
    }

    public void OnHeroMove()
    {
        if(skipStep == true)
        {
            skipStep = false;
        }
        else
        {
            if(!enemyAI)
            {
                return;
            }

            foreach(var enemy in enemyList)
            {
                enemy.AI();
            }
            skipStep = true;
        }
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene("Main"); // Reload current scene
    }

    public void GameOver()
    {
        SoundManager.instance.PlaySingle(gameOverSound);
        SoundManager.instance.musicSource.Stop();
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitGame();
        level++;
    }

}
