using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tokenplacer : MonoBehaviour {

    public Transform token;

	// Use this for initialization
	void Start () {
        System.Console.Out.WriteLine("abc");
        Instantiate(token, new Vector3(0f, 0.1f, 0f), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
