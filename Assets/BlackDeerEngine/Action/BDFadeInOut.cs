using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDFadeInOut : MonoBehaviour {
	public UnityEngine.UI.Image fade;
	float fades = 1.0f;
	float time = 0;
	float period = 1.0f;
	int fadeMode = 0; // 0: stop / 1: fadein / 2: fadeout
	// Use this for initialization
	void Start () {
		fade = gameObject.GetComponent<UnityEngine.UI.Image>();
		if (fade == null) {
			Debug.Log("BDFadeInOut: can't find component <UnityEngine.UI.Image>");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fade != null && fadeMode != 0) {
			if (time > 0) {
				time -= Time.deltaTime;
				if (fadeMode == 1) {
					fades = time / period;
				}else if (fadeMode == 2) {
					fades = 1.0f - time / period;
				}
				fade.color = new Color ( 0, 0, 0, fades);
			}else {
				fadeMode = 0;
				time = 0;
			}
		}
	}

	public void startFadeIn(float period) {
		if (period == 0) {
			period = 0.1f;
		}
		fadeMode = 1;
		this.period = period;
		this.time = period;
	}

	public void startFadeOut(float period) {
		if (period == 0) {
			period = 0.1f;
		}
		fadeMode = 2;
		this.period = period;
		this.time = period;
	}
}
