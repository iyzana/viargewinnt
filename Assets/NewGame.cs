using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class NewGame : MonoBehaviour {

    public Button startButton;
    public Text spieler;
    public Text gameIdText;

    // Use this for initialization
    void Start() {
        startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        gameIdText.text = "Game-Id: "+HttpService.gameId;
    }

    // Update is called once per frame
    void Update() {

    }

    void StartGame(){

        HttpService.HttPost("start/" + HttpService.gameId);
        GameInformation.GAMEMODE = "ONLINE";
        SceneManager.LoadScene("viargewinnt-scene", LoadSceneMode.Single);
    }

}
