using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgramManager : MonoBehaviour {

	HeapPriorityQueue heap;
	public GameObject node;
	private Transform rootPos;
	public float delay = 1f;
	public int numberOfNodes = 63;

	public InputField manualInput;
	public Dropdown delayInput;
	public Button addRandomBtn;
	public Button removeBtn;
	public GameObject cover;
	public Toggle autoAddToggle;
	public Toggle autoRemoveToggle;
	public Toggle muteToggle;

	public bool autoAdd = false;
	public bool autoRemove = false;

	// Use this for initialization
	void Start () {
		rootPos = GameObject.Find ("Root").transform;
		heap = GetComponent<HeapPriorityQueue> ();
		heap.Initialize (delay);

		manualInput.onEndEdit.AddListener (SubmitValue);
		delayInput.onValueChanged.AddListener (delegate{ChangeDelay();});
		addRandomBtn.onClick.AddListener (AddRandom);
		removeBtn.onClick.AddListener (Remove);
		autoAddToggle.onValueChanged.AddListener (delegate{AutoInsertToggle();});
		autoRemoveToggle.onValueChanged.AddListener (delegate{AutoRemoveToggle();});
		muteToggle.onValueChanged.AddListener (delegate {Mute ();});

		delayInput.value = 3;

		StartCoroutine (AutoInsert());
		StartCoroutine (AutoRemove ());
	}

	void SubmitValue(string arg0)
	{
		if (!heap.inProgress && heap.Size() < numberOfNodes) {
			if (manualInput.text != "") {
				GameObject.Find ("SoundEffects").GetComponent<SoundEffects> ().New ();
				GameObject u = Instantiate (node, rootPos) as GameObject;
				u.GetComponent<RectTransform> ().localScale = Vector3.one;
				u.GetComponent<Node> ().SetData (int.Parse (arg0));
				u.name = u.GetComponent<Node> ().data + "  -  Node" + heap.Size ();
				heap.StartCoroutine ("Insert", u);
				
				manualInput.text = "";
			}
		}
	}

	void ChangeDelay()
	{
		delay = delayInput.value;
		heap.SetDelay (delay);
	}

	void AutoRemoveToggle()
	{
		autoRemove = autoRemoveToggle.isOn;
		if (autoAdd == true) {
			autoAdd = false;
			autoAddToggle.isOn = autoAdd;
		}

	}

	void AutoInsertToggle()
	{
		autoAdd = autoAddToggle.isOn;
		if (autoRemove == true) {
			autoRemove = false;
			autoRemoveToggle.isOn = autoRemove;
		}
	}

	void Mute()
	{
		GameObject.Find ("SoundEffects").GetComponent<AudioSource> ().mute = muteToggle.isOn;
	}

	IEnumerator AutoInsert()
	{
		while (true) {
			if (!heap.inProgress && autoAdd) {
				AddRandom ();
			}
			while (heap.inProgress)
				yield return null;
			yield return new WaitForSeconds (delay);

		}
	}

	IEnumerator AutoRemove()
	{
		while (true) {
			if (!heap.inProgress && autoRemove) {
				Remove ();
			}
			while (heap.inProgress)
				yield return null;
			yield return new WaitForSeconds (delay);

		}
	}

	void AddRandom()
	{
		if (!heap.inProgress && heap.Size() < numberOfNodes) {
			GameObject.Find ("SoundEffects").GetComponent<SoundEffects> ().New ();
			GameObject u = Instantiate (node, rootPos) as GameObject;
			u.GetComponent<RectTransform> ().localScale = Vector3.one;
			u.GetComponent<Node> ().SetRandomData ();
			u.name = u.GetComponent<Node> ().data + "  -  Node" + heap.Size ();
			heap.StartCoroutine ("Insert", u);
		}
	}

	void Remove()
	{
		if (!heap.inProgress && !heap.Empty()) {
			heap.StartCoroutine ("RemoveMin");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!heap.inProgress) {

			cover.SetActive (false);

			if (Input.GetKeyDown (KeyCode.Space)) {
				AddRandom ();
			}
			
			if (Input.GetKeyDown (KeyCode.B) && !heap.Empty ()) {
				Remove ();
			}
		} else {
			cover.SetActive (true);
		}

	}

}
