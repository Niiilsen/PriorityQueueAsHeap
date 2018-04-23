using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeapPriorityQueue : MonoBehaviour {

	private TreeStructure tree;
	public float delay = 1f;

	public bool inProgress = false;
	public Text listPrint;

	public void Initialize(float _delay){
		delay = _delay;
		tree = GetComponent<TreeStructure> ();
		tree.Initialize ();
	}

	public void SetDelay(float _delay){
		delay = _delay;
	}

	public int Size()
	{
		return tree.Size ();
	}

	public bool Empty()
	{
		return Size () == 0;
	}

	public IEnumerator Insert(GameObject e)
	{	
		inProgress = true;
		tree.AddLast (e);
		GameObject v = tree.Last ();
		v.GetComponent<Animator> ().SetTrigger ("Activate");
		WriteList ();

		while (!tree.IsRoot (v)) {
			GameObject u = tree.Parent (v);
			yield return new WaitForSeconds (delay/2);
			if (!IsLess (v, u)) {
				u.GetComponent<Animator> ().SetTrigger ("PulseRed");
				GameObject.Find ("SoundEffects").GetComponent<SoundEffects> ().NotAllowed ();
				yield return new WaitForSeconds (delay / 2);
				break;
			}

			GameObject.Find ("SoundEffects").GetComponent<SoundEffects> ().Allowed ();
			u.GetComponent<Animator> ().SetTrigger ("PulseGreen");
			yield return new WaitForSeconds (delay/2);
			tree.Swap (v, u);
			WriteList ();
			// v = u 	//Ikke nødvendig siden vi ikke bruker pekere
		}

		WriteList ();

		//yield return new WaitForSeconds (delay/2);
		v.GetComponent<Animator> ().SetTrigger ("Deactivate");
		inProgress = false;
	}

	public GameObject Min()
	{
		return tree.Root ();
		
	}

	public IEnumerator RemoveMin()
	{
		inProgress = true;
		if (Size () == 1) {
			tree.removeLast ();
		} else {
			GameObject u = tree.Root ();

			// Noden som jobbes med blir grå
			tree.Last().GetComponent<Animator>().SetTrigger("Activate");

			tree.Swap (u, tree.Last ());
			WriteList ();
			yield return new WaitForSeconds (delay/4);
			u = tree.Root (); // Reassigner u siden den ikke er en peker


			tree.Last().GetComponent<Animator> ().SetTrigger ("Death");
			GameObject.Find ("SoundEffects").GetComponent<SoundEffects> ().Death ();
			yield return new WaitForSeconds (delay);


			tree.removeLast ();
			WriteList ();
			while (tree.HasLeft (u)) {
				GameObject v = tree.Left (u);
				if (tree.HasRight (u) && IsLess (tree.Right (u), v)) {
					v = tree.Right (u);
				}

				yield return new WaitForSeconds (delay);
				if (IsLess (v, u)) {

					if (v != tree.Left(u)) {
						tree.Left (u).GetComponent<Animator> ().SetTrigger ("PulseRed");
						GameObject.Find ("SoundEffects").GetComponent<SoundEffects> ().NotAllowed ();

						yield return new WaitForSeconds (delay / 2);
					}
					v.GetComponent<Animator> ().SetTrigger ("PulseGreen");
					GameObject.Find ("SoundEffects").GetComponent<SoundEffects> ().Allowed ();
					yield return new WaitForSeconds (delay/4);
					tree.Swap (u, v);
					WriteList ();
					//u = v;
				} 
				else
				{
					v.GetComponent<Animator> ().SetTrigger ("PulseRed");
					GameObject.Find ("SoundEffects").GetComponent<SoundEffects> ().NotAllowed ();
					yield return new WaitForSeconds (delay/2);
					break;
				}
			}
			u.GetComponent<Animator>().SetTrigger("Deactivate");
		}

		WriteList ();
		inProgress = false;
	}
		

	//Comparator
	bool IsLess(GameObject v, GameObject u){
		//print ("Compare: " + v.GetComponent<Node> ().data + " < " + u.GetComponent<Node> ().data + "  " + (v.GetComponent<Node> ().data < u.GetComponent<Node> ().data));
		return (v.GetComponent<Node> ().data < u.GetComponent<Node> ().data);
	}


	void WriteList()
	{
		listPrint.text = "| "; 
		for (int i = 1; i <= Size(); i++) {
			listPrint.text += tree.V[i].GetComponent<Node>().data.ToString() + " | ";	
		}
	}
}
