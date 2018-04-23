using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TreeStructure : MonoBehaviour
{

	public List<GameObject> V = new List<GameObject>{ null };
	private List<GameObject> lines = new List<GameObject> ();
	public float Yscalar = 10f;
	public float Xscalar = 10f;
	public GameObject line;
	public float lineThickness = 1f;

	public void Initialize()
	{
		Yscalar = -Screen.height / 12f;
		Xscalar = Screen.width / 1.3f;
	}


	public GameObject Pos (int i)
	{
		return V [i];
	}

	int idx (GameObject p)
	{
		return V.IndexOf (p);
	}


	public int Size ()
	{
		return V.Count - 1;	
	}

	public GameObject Left (GameObject p)
	{
		return Pos (2 * idx (p));
	}

	public GameObject Right (GameObject p)
	{
		return Pos (2 * idx (p) + 1);
	}

	public GameObject Parent (GameObject p)
	{
		return Pos (idx (p) / 2);
	}

	public bool HasLeft (GameObject p)
	{
		return (2 * idx (p) <= Size ());
	}

	public bool HasRight (GameObject p)
	{
		return (2 * idx (p) + 1 <= Size ());
	}

	public bool IsRoot (GameObject p)
	{
		return idx (p) == 1;
	}

	public GameObject Root ()
	{
		return Pos (1);
	}

	public GameObject Last ()
	{
		return Pos (Size ());
	}

	public void AddLast (GameObject e)
	{
		V.Insert (Size () + 1, e);
		CalculatePosition (e);
		CreateLine ();
	}

	public void removeLast ()
	{
		if (Size() > 0) {
			//Remove and destroy Node
			GameObject gameObj = V [Size ()];
			V.RemoveAt (Size ()); // Remove from tree
			Destroy (gameObj);
		}

		if (lines.Count > 0) {
			// Remove and destroy line
			GameObject lineObj = lines [lines.Count - 1];
			lines.RemoveAt (lines.Count - 1); // Remove from lines
			Destroy (lineObj);
		}
	}

	public void Swap (GameObject p, GameObject q)
	{
		GameObject temp = q;
		int tempIdx = idx (p);

		V [idx (q)] = p; 
		V [tempIdx] = temp;


		CalculatePosition (V [idx (q)]);
		CalculatePosition (V [idx (p)]);
	}

	public void CalculatePosition (GameObject e)
	{
		
		float rows = Mathf.Floor (Mathf.Log (idx (e), 2)) + 1;
		float nodesInRow = Mathf.Pow (2, rows - 1); 
		float totalPossibleNodes = 0;
		for (int j = 0; j < rows; j++) {
			totalPossibleNodes += Mathf.Pow (2, j);
		}

		float y = Mathf.Floor (Mathf.Log (idx (e), 2)) + 1;
		float x = (-nodesInRow / 2) + (Mathf.Abs (idx (e) - totalPossibleNodes)+0.5f);
		Vector3 newPos = Pos (idx (e)).transform.parent.GetComponent<RectTransform> ().position + new Vector3 (-x * Xscalar/nodesInRow, y * Yscalar, 0);
		Pos (idx (e)).GetComponent<Node> ().SetPos (newPos);
	}

	public void CreateLine()
	{
		if (Size() > 1) {
			GameObject newLine = Instantiate (line, GameObject.Find("LineParent").transform) as GameObject;
			newLine.GetComponent<RectTransform> ().pivot = new Vector2 (0, 0.5f); 
			newLine.GetComponent<RectTransform> ().position = Last ().GetComponent<Node>().targetPos;
			Vector3 differenceVector = Parent (Last ()).GetComponent<Node>().targetPos - newLine.GetComponent<RectTransform> ().position; 
			newLine.GetComponent<RectTransform> ().sizeDelta = new Vector2 (differenceVector.magnitude, lineThickness);
			float angle = Mathf.Atan2 (differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
			newLine.GetComponent<RectTransform> ().rotation = Quaternion.Euler (0, 0, angle);
			lines.Add (newLine);
		}
	}
}
