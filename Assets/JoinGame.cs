using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class JoinGame : MonoBehaviour {

    public InputField gameIdInput;
    public InputField nameInput;
    public Button joinGameButton;

	// Use this for initialization
	void Start () {
        joinGameButton.GetComponent<Button>().onClick.AddListener(JoinGameStart);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void JoinGameStart(){
        HttpService.HttPost("join/" + gameIdInput.text + "/"+nameInput.text);
        SceneManager.LoadScene("viargewinnt-lobby-waiting-scene", LoadSceneMode.Single);
    }
}
