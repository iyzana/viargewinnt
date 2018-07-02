using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitGame : MonoBehaviour {

    public InputField input;
    public Button initButton;

    // Use this for initialization
    void Start () {
        initButton.GetComponent<Button>().onClick.AddListener(init);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void init()
    {
        string gameId = HttpService.HttPost("create");
        HttpService.HttPost("join/" + gameId + "/" + input.text);
        HttpService.gameId = gameId;
        HttpService.player = input.text;
        HttpService.startSocket();
        SceneManager.LoadScene("viargewinnt-lobby-newgame-scene", LoadSceneMode.Single);
    }
}
