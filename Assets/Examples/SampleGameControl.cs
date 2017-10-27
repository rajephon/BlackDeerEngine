using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGameControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BlackDeerEngine.Instance.DelegateStageClear = delegate(BlackDeerEngine.GameProgress gameProgress) {
			Debug.Log(gameProgress.stage+ " STAGE CLEAR");
		};
		BlackDeerEngine.Instance.DelegateBlockUserControl = delegate(bool uiblocked) {
			Debug.Log("UIBlockedMode: "+uiblocked);
		};
		BlackDeerEngine.Instance.DelegateLoadComplete = delegate() {
			BlackDeerEngine.Instance.setProgressStep(1, "설원", 2, 1);
			Debug.Log("Load Complete");
			BlackDeerEngine.Instance.startProgress();
		};
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
