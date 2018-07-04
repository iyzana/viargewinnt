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

    private WebSocket w;

    // Use this for initialization
    void Start() {
        startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        gameIdText.text = "Game-Id: "+HttpService.gameId;
        w = HttpService.w;
    }

    // Update is called once per frame
    void Update()
    {
        CheckWebsocket();

    }

    private void CheckWebsocket()
    {
        string reply = w.RecvString();
        if (reply == null)
        {
            return;
        }

        Debug.Log("got websocket message: " + reply);
        TurnEvent turnEvent = JsonUtility.FromJson<TurnEvent>(reply);
        updatePlayers(turnEvent);
    }

    private void updatePlayers(TurnEvent turnEvent)
    {
        string players = "";
        for(int i = 0; i < turnEvent.game.players.GetLength(0); i++)
        {
            players += turnEvent.game.players[i] + "\n";
        }
        spieler.text = players;
    }

    void StartGame(){

        HttpService.HttPost("start/" + HttpService.gameId);
        GameInformation.GAMEMODE = "ONLINE";
        SceneManager.LoadScene("viargewinnt-scene", LoadSceneMode.Single);
    }

}
