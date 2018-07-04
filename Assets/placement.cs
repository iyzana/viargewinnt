using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class placement : MonoBehaviour {
    private bool isLocal = false;

    public Button leftButton;
    public Button rightButton;
    public Button placeButton;

    public Text text;

    public Transform tokenLightPrefab;
    public Transform tokenDarkPrefab;
    public Transform tokenPlaceLightPrefab;
    public Transform tokenPlaceDarkPrefab;

    public Transform tracked;
    private Transform token;

    public string[,] field = new string[7, 6];

    private long lastChange = 0;
    private int lastPosition = -2;
    private int fieldPosition = -1;
    private bool nextColor = true;

    private Vector3 lastVector = new Vector3(-1, 0, 0);
    private WebSocket w;

    // Use this for initialization
    void Start() {

        

        for (int x = 0; x < field.GetLength(0); x++)
        {
            for (int y = 0; y < field.GetLength(1); y++)
            {
                field[x, y] = "";
            }
        }
        isLocal = GameInformation.GAMEMODE.Equals("LOCAL");
        token = Instantiate(tokenPlaceLightPrefab, new Vector3(0f, 0.65f, 0f), Quaternion.identity);

        leftButton.GetComponent<Button>().onClick.AddListener(LeftClick);
        rightButton.GetComponent<Button>().onClick.AddListener(RightClick);
        placeButton.GetComponent<Button>().onClick.AddListener(PlaceClick);

        if (isLocal)
        {
            text.text = "Spieler 1 beginnt!";
        }
        else
        {
            w = HttpService.w;
        }
        
    }
    
    void LeftClick() {
        fieldPosition--;
        if (fieldPosition < 0) fieldPosition = 0;
        else if (fieldPosition > 6) fieldPosition = 6;
    }

    void RightClick() {
        fieldPosition++;
        if (fieldPosition < 0) fieldPosition = 0;
        else if (fieldPosition > 6) fieldPosition = 6;
    }

    void PlaceClick() {
        PlaceAt(fieldPosition);
    }
   

    // Update is called once per frame
    void Update () {
        if (!isLocal)
        {
            CheckWebsocket();
        }

        bool trackingPositionChanged = !lastVector.Equals(tracked.position);
        if (trackingPositionChanged) {
            fieldPosition = (int)(tracked.position.x * 30f) + 3;
            if (fieldPosition < -1) fieldPosition = -1;
            else if (fieldPosition > 7) fieldPosition = 7;
        }
        
        if (lastPosition != fieldPosition) {
            lastChange = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        } else if (lastPosition >= 0 && lastPosition <= 6 && trackingPositionChanged) {
            long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (currentTime - lastChange > 2000) {
                PlaceAt(fieldPosition);
                lastChange = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
        }

        token.position = new Vector3((fieldPosition - 3) * 0.1f, 0.65f, 0);
        lastPosition = fieldPosition;
        lastVector = new Vector3(tracked.position.x, tracked.position.y, tracked.position.z);
    }

    void PlaceAt(int x) {
        if (!isLocal)
        {
            HttpService.HttPost("turn/" + HttpService.gameId + "/"+HttpService.player+"/" + x);
        }
        else
        {
            for (int y = 5; y >= 0; y--)
            {
                if (field[x, y].Equals(""))
                {
                    field[x, y] = nextColor ? "Spieler1" : "Spieler2";
                    if (WinDetection.isWon(field))
                    {
                        GameInformation.WINNINGTEXT = field[x, y] + " hat gewonnen!!";
                        SceneManager.LoadScene("viargewinnt-winning-scene", LoadSceneMode.Single);
                        break;
                    }

                    Transform material = nextColor ? tokenLightPrefab : tokenDarkPrefab;
                    Instantiate(material, new Vector3((x - 3) * 0.1f, (5 - y) * 0.1f + 0.05f, 0f), Quaternion.identity);
                    nextColor = !nextColor;
                    Destroy(token.gameObject);
                    Transform placeMaterial = nextColor ? tokenPlaceLightPrefab : tokenPlaceDarkPrefab;
                    text.text = nextColor ? "Spieler1 ist am Zug!" : "Spieler2 ist am Zug!";
                    token = Instantiate(placeMaterial, new Vector3(0f, 0.65f, 0f), Quaternion.identity);
                    break;

                }
            }
        }
        
    }

    private void CheckWebsocket()
    {
        if (w == null)
        {
            return;
        }
        string reply = w.RecvString();
        if (reply == null)
        {
            return;
        }

        Debug.Log("got websocket message: " + reply);
        TurnEvent turnEvent = JsonUtility.FromJson<TurnEvent>(reply);
        renderChanges(turnEvent);
    }

    private void renderChanges(TurnEvent turnEvent)
    {
        
        if (turnEvent.win)
        {
            GameInformation.WINNINGTEXT = turnEvent.previousPlayer + " hat gewonnen!!";
            SceneManager.LoadScene("viargewinnt-winning-scene", LoadSceneMode.Single);
        }
        else
        {
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (turnEvent.game.grid[x].pieces[y]!=null && !field[x, y].Equals(turnEvent.game.grid[x].pieces[y]))
                    {
                        field[x, y] = turnEvent.game.grid[x].pieces[y];
                        Transform material = turnEvent.game.grid[x].pieces[y].Equals(HttpService.player) ? tokenLightPrefab : tokenDarkPrefab;
                        Instantiate(material, new Vector3((x - 3) * 0.1f, (5 - y) * 0.1f + 0.05f, 0f), Quaternion.identity);
                        text.text = turnEvent.game.currentPlayer+" ist am Zug!";
                    }
                }
            }
        }
        
    }

    
}
