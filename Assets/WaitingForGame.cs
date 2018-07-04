using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//TODO Ins Spiel leiten, wenn TurnEvent
public class WaitingForGame : MonoBehaviour {

    private WebSocket w;

    // Use this for initialization
    void Start () {
        w = HttpService.w;
		
	}
	
	// Update is called once per frame
	void Update () {
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
        goToGame(turnEvent);
    }

    private void goToGame(TurnEvent turnEvent)
    {
        if (turnEvent.type.Equals("GAMESTART"))
        {
            GameInformation.GAMEMODE = "ONLINE";
            SceneManager.LoadScene("viargewinnt-scene", LoadSceneMode.Single);
        }
        
    }
}
