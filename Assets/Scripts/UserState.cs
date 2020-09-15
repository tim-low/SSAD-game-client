using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UserState : MonoBehaviour {
    public string Token;
    public byte[] characterWears = {8,8,8,8 };
    public string username;
    public int chestCount=0;
	// Use this for initialization
	void Start () {
        Token = "Mdf9QfvQSvDqQV2os+PuF3UiJIMKl/bpB1+uXxNi9LA=";
    }

    private static UserState _instance = null;

    public static bool Exists()
    {
        return _instance != null;
    }

    public static UserState Instance()
    {
        if (!Exists())
        {
            throw new Exception("UserState could not find the UserState object. Please ensure you have added the UserState Prefab to your scene.");
        }
        return _instance;
    }


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    
}
