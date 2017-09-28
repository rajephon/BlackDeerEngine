using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BDChatMessage : MonoBehaviour {
	static Canvas canvas = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	static public void setCanvas(Canvas canvas) {
		BDChatMessage.canvas = canvas;
	}

	static public GameObject createChatbubble(GameObject targetObj, string message) {
		if (canvas == null) {
			Debug.Log("canvas == null");
			return null;
		}
		if (targetObj == null) {
			Debug.Log("targetObj == null");
			return null;
		}
		GameObject chatBoxObj = new GameObject();
		Text txtComponent = chatBoxObj.gameObject.AddComponent<Text>();
		txtComponent.name = targetObj.name + "_chatbox";
		txtComponent.text = message;
		txtComponent.font = (Font)Resources.Load("Fonts/NanumGothic");
		txtComponent.color = new Color(0,0,0,1);
		txtComponent.alignment = TextAnchor.MiddleCenter;

		Outline outline = chatBoxObj.gameObject.AddComponent<Outline>();
		outline.effectColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

		Debug.Log(targetObj.transform.position);
		Vector3 convertedPosition = Camera.main.WorldToScreenPoint(targetObj.transform.position);
		chatBoxObj.transform.position = new Vector3(convertedPosition.x, convertedPosition.y+40.0f);
		// size
		chatBoxObj.GetComponent<RectTransform>().sizeDelta = new Vector2(240, 60);
		
		txtComponent.enabled = false;

		chatBoxObj.transform.parent = canvas.transform;
		return chatBoxObj;
	}
}
