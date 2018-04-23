using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListTest : MonoBehaviour {
	public GameObject testObj;
	public List<GameObject> list = new List<GameObject>{null, null};


	// Use this for initialization
	void Start () {
		list [1] = testObj;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
