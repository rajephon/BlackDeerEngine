using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGameControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BlackDeerEngine.DelegateStageClear = delegate(BlackDeerEngine.GameProgress gameProgress) {
			Debug.Log(gameProgress.stage+ " STAGE CLEAR");

		};
		BlackDeerEngine.DelegateBlockUserControl = delegate(bool uiblocked) {
			Debug.Log("UIBlockedMode: "+uiblocked);
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
