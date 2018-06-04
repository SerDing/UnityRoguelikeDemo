using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMgr : MonoBehaviour {

    public GameObject[] outWallArray;

    public GameObject[] floorArray;

    public GameObject[] wallArray;

    public GameObject[] foodArray;

    public GameObject[] enemyArray;

    public GameObject exitPrefab;

    // scene scale 10*10
    public int sceneRows = 10;

    public int sceneCols = 10;

    private Transform sceneHolder;

    private List<Vector2> positionList = new List<Vector2>();

    public int wallMinCount = 2;

    public int wallMaxCount = 8;

    private GameMgr gameMgr;

    void Awake()
    {
        gameMgr = this.GetComponent<GameMgr>();
        InitScene();
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Init a scene 
    public void InitScene() {

        sceneHolder = new GameObject("Map").transform;
        
        // create outWall and  floor  
        for (int x = 0; x < sceneCols; x++)
        {
            for (int y = 0; y < sceneRows; y++)
            {
                if (x == 0 || y == 0 || x == sceneCols - 1 || y == sceneRows - 1)
                {
                    GameObject outWallPrefab = GetRandomPrefab(outWallArray);

                    GameObject go = GameObject.Instantiate(outWallPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                    go.transform.SetParent(sceneHolder);
                }
                else
                {
                    GameObject floorPrefab = GetRandomPrefab(floorArray);

                    GameObject go = GameObject.Instantiate(floorPrefab, new Vector3(x,y,0),Quaternion.identity) as GameObject;

                    go.transform.SetParent(sceneHolder);
                }
            }

        }

        // Get avaliable area position list

        positionList.Clear();

        for (int x = 2; x < sceneCols - 2; x++)
        {
            for (int y = 2; y < sceneRows - 2; y++)
            {
                positionList.Add(new Vector2(x, y));
            }
        }

        // create obcastle 

        int wallCount = Random.Range(wallMinCount,wallMaxCount + 1);

        InstantiateItems(wallCount, wallArray);

        // create food

        int foodCount = Random.Range(2,gameMgr.level * 2 + 1);

        InstantiateItems(foodCount,foodArray);

        // create enemy!!!

        int enemyCount;

        if (gameMgr.level == 1)
        {
            enemyCount = 1;
        }
        else
        {
            enemyCount = gameMgr.level / 2;
        }

        InstantiateItems(enemyCount,enemyArray);

        // create exit door..

        GameObject go3 = GameObject.Instantiate(exitPrefab,new Vector2(sceneCols - 2,sceneRows - 2),Quaternion.identity) as GameObject;

        go3.transform.SetParent(sceneHolder);

    }

    private void InstantiateItems(int count , GameObject[] prefabs)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = GetRandomPrefab(prefabs);

            Vector2 pos = GetRandomPosition();

            GameObject go = GameObject.Instantiate(prefab,pos,Quaternion.identity) as GameObject;

            go.transform.SetParent(sceneHolder);
        }
    }

    private Vector2 GetRandomPosition() {

        int positionIndex = Random.Range(0, positionList.Count);

        Vector2 pos = positionList[positionIndex];

        positionList.RemoveAt(positionIndex);

        return pos;

    }

    private GameObject GetRandomPrefab(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);

        return prefabs[index];
    }


}
