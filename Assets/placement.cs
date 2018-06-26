using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class placement : MonoBehaviour {

    public Button leftButton;
    public Button rightButton;
    public Button placeButton;

    public Transform tokenLightPrefab;
    public Transform tokenDarkPrefab;
    public Transform tokenPlaceLightPrefab;
    public Transform tokenPlaceDarkPrefab;

    public Transform tracked;
    private Transform token;

    public bool[,] field = new bool[7,6];
    
    private long lastChange = 0;
    private int lastPosition = -2;
    private int fieldPosition = -1;
    private bool nextColor = true;

    private Vector3 lastVector = new Vector3(-1, 0, 0);
    private string gameId;
    private const string baseUrl = "http://localhost:4567/";
    private WebSocket w;

    // Use this for initialization
    void Start () {
        token = Instantiate(tokenPlaceLightPrefab, new Vector3(0f, 0.65f, 0f), Quaternion.identity);

        leftButton.GetComponent<Button>().onClick.AddListener(LeftClick);
        rightButton.GetComponent<Button>().onClick.AddListener(RightClick);
        placeButton.GetComponent<Button>().onClick.AddListener(PlaceClick);
        
        gameId = HttPost(baseUrl + "create");
        HttPost(baseUrl + "join/" + gameId + "/peter");
        HttPost(baseUrl + "start/" + gameId);

        w = new WebSocket(new Uri("wss://localhost:4567/state"));
        StartCoroutine(w.Connect());
        w.SendString(gameId);

        ThreadStart work = WebSocketListener;
        Thread thread = new Thread(work);
        thread.Start();
    }

    class TurnEvent {
        Game game;
        string previousPlayer;
        bool win;
    }

    class Game {
        string state;
        long id;
        string[] players;
        string currentPlayer;
        int width;
        int height;
        string[,] grid;
    }

    void WebSocketListener() {
        while (true) {
            string reply = w.RecvString();

            if (reply == null)
                break;

            var turnEvent = JsonUtility.FromJson<TurnEvent>(reply);
            Debug.Log(turnEvent);
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
    
    static string HttPost(string url, string content = "") {
        HttpWebRequest req = WebRequest.Create(url)
                                as HttpWebRequest;
        req.Method = "POST";
        req.ContentType = "appliaction/json";

        var newStream = req.GetRequestStream();
        var data = Encoding.ASCII.GetBytes(content);
        newStream.Write(data, 0, data.Length);
        newStream.Close();

        string result = null;
        using (HttpWebResponse resp = req.GetResponse()
                                        as HttpWebResponse) {
            StreamReader reader =
                new StreamReader(resp.GetResponseStream());
            result = reader.ReadToEnd();
        }
        return result;
    }

    // Update is called once per frame
    void Update () {
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
        HttPost(baseUrl + "turn/" + gameId + "/peter/" + x);
        for (int y = 5; y >= 0; y--) {
            if (field[x, y] == false) {
                field[x, y] = true;
                Transform material = nextColor ? tokenLightPrefab : tokenDarkPrefab;
                Instantiate(material, new Vector3((x - 3) * 0.1f, (5 - y) * 0.1f + 0.05f, 0f), Quaternion.identity);
                nextColor = !nextColor;
                Destroy(token.gameObject);
                Transform placeMaterial = nextColor ? tokenPlaceLightPrefab : tokenPlaceDarkPrefab;
                token = Instantiate(placeMaterial, new Vector3(0f, 0.65f, 0f), Quaternion.identity);
                break;
            }
        }
    }
}
