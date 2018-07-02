using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinningScript : MonoBehaviour {

    public Text text;
    public Button newGameButton;
    public Button mainMenuButton;

    // Use this for initialization
    void Start () {
        text.text = GameInformation.WINNINGTEXT;
        newGameButton.GetComponent<Button>().onClick.AddListener(NewGame);
        mainMenuButton.GetComponent<Button>().onClick.AddListener(ToMainMenu);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void NewGame()
    {
        GameInformation.WINNINGTEXT = "";
        SceneManager.LoadScene("viargewinnt-scene", LoadSceneMode.Single);
    }

    void ToMainMenu()
    {
        SceneManager.LoadScene("viargewinnt-lobby-scene", LoadSceneMode.Single);
    }



}
