using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDCameraAnimCallback : MonoBehaviour {
	private BDAction.CompletionDelegate completionDelegate = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void animationFinish() {
		Debug.Log("AnimationFinish");
		if (completionDelegate != null) {
			completionDelegate();
		}
	}
}
