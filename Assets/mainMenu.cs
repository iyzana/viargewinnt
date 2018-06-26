using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

    public Button localGame;
    public Button startOnline;
    public Button joinOnline;

	// Use this for initialization
	void Start () {
        localGame.GetComponent<Button>().onClick.AddListener(StartLocalGame);
        startOnline.GetComponent<Button>().onClick.AddListener(StartOnlineGame);
        joinOnline.GetComponent<Button>().onClick.AddListener(JoinOnlineGame);
    }
    
    void StartLocalGame() {
         SceneManager.LoadScene("viargewinnt-scene", LoadSceneMode.Single);
    }
    
    void StartOnlineGame(){
        
    }
    
    void JoinOnlineGame(){
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
