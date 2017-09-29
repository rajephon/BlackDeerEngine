using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
			this.setEnabled(false);
			this.setHidden(true);
			completionDelegate();
			// completionDelegate = null;
		}else {
			Debug.Log("onClicked. but completionDelegate is null");
		}
		// Debug.Log("You have clicked the button!");
	}

	public void setCompletionDelegate(BDAction.CompletionDelegate completionDelegate) {
		this.completionDelegate = completionDelegate;
	}

	public void setEnabled(bool isEnabled) {
		// htisinteractiable
		this.GetComponent<Button>().interactable = isEnabled;
	}

	public void setHidden(bool isHide) {
		this.gameObject.SetActive(!isHide);
	}
}
