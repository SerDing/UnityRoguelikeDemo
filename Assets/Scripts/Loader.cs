using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameMgr;

    void Awake()
    {
        if(GameMgr._instance == null)
        {
            Instantiate(gameMgr);
        }
        
    }
}
