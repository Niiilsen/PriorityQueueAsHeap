using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Node : MonoBehaviour {

	public int data;
	public Vector3 targetPos;
	public bool posSet = false;
	public float moveSpeed = 1000;

	void Start()
	{
		moveSpeed = Screen.width/1.6f;
	}

	public void SetRandomData(){
		data = Random.Range (0, 100);
		transform.Find("Text").GetComponent<Text> ().text = data.ToString ();
	}

	public void SetPos(Vector3 pos){
		if (posSet)
			targetPos = pos;
		else {
			transform.position = pos;
			targetPos = pos;
			posSet = true;
		}
	}

	public void SetData(int _data){
		data = _data;
		transform.Find("Text").GetComponent<Text> ().text = data.ToString ();
	}

	void Update()
	{
		transform.position = Vector3.MoveTowards (transform.position, targetPos, moveSpeed*Time.deltaTime);
	}
}
