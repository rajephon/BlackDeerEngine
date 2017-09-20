using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDNextActionButton : MonoBehaviour {
	BDAction.CompletionDelegate completionDelegate = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void onClicked() {
		if (completionDelegate != null) {
			Debug.Log("onClicked");
			completionDelegate();
			completionDelegate = null;
		}else {
			Debug.Log("onClicked. but completionDelegate is null");
		}
		// Debug.Log("You have clicked the button!");
	}

	public void setCompletionDelegate(BDAction.CompletionDelegate completionDelegate) {
		this.completionDelegate = completionDelegate;
	}
}
