using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System.Xml;
using System.Text;
using System;

public class BlackDeerEngine : MonoBehaviour {
	// UISetting
	public GameObject fadePanel = null;
	public Camera mainCamera = null;
	public Text txtChatbox = null;
	public Button buttonNextAction = null;

	public Canvas mainCanavas = null;
	public class GameProgress {
		public int stage;
		public int episode;
		public string place;
		public int scene;
		public int action;
		public GameProgress(int stage, int episode, string place, int scene, int action) {
			this.stage = stage;
			this.episode = episode;
			this.place = place;
			this.scene = scene;
			this.action = action;
		}
		public string getXPath() {
			return getXPath(stage, episode, place, scene, action);
		}
		public string getXPath(int stage, int episode, string place, int scene, int action) {
			return "stage[@value=\""+stage+"\"]/episode[@value = \""+episode+"\"]/place[@name=\""+place+"\"]/scene[@no=\""+scene+"\"]/action["+action+"]";
		}
	}
	public string m_strName = "xml/SampleEpisode.xml";
	private static GameProgress gameProgress;
	private XmlDocument xmlDoc;
	// Use this for initialization
	void Start () {
		gameProgress = new GameProgress(1, 1, "설원", 1, 1);
		StartCoroutine(LoadXMLResource());
		initEnvironment();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void initEnvironment() {
		if (mainCanavas != null) {
			RectTransform canvasRect = mainCanavas.gameObject.GetComponent<RectTransform>();

			// create fadePanel
			GameObject fadePanel = new GameObject();
			fadePanel.name = "BDFadePanel";
			Image NewImage = fadePanel.AddComponent<Image>();
			// NewObj.GetComponent<RectTransform>().SetParent(mainCanavas.game); //Assign the newly created Image GameObject as a Child of the Parent Panel.
			fadePanel.transform.SetParent(mainCanavas.transform);
			fadePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasRect.rect.width, canvasRect.rect.height);
			fadePanel.GetComponent<RectTransform>().localPosition = new Vector2(0,0);
			fadePanel.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(0,0,0,0.0f);
			fadePanel.SetActive(false); //Activate the GameObject
			this.fadePanel = fadePanel;
		}else {
			Debug.LogError("mainCanvas == null");
		}
		if (fadePanel != null) {
			BDFadeInOut bdFadeInOut = fadePanel.AddComponent<BDFadeInOut>();
			BDActionFadeInOut.setFadePanel(bdFadeInOut);
		}else {
			Debug.LogError("fadePanel == null");
		}

		BDCameraAnimCallback cameraAnimCallbackListener = mainCamera.gameObject.AddComponent<BDCameraAnimCallback>();

		// BDActionCameraMove.setMainCamera(mainCamera);
		if (buttonNextAction != null) {
			BDNextActionButton nextActionButton = buttonNextAction.gameObject.AddComponent<BDNextActionButton>();
			buttonNextAction.onClick.AddListener(nextActionButton.onClicked);
			BDActionChatMessage.setTxtChatbox(txtChatbox, nextActionButton);
			BDActionChatEnable.setTxtChatbox(txtChatbox, nextActionButton);
		}else {
			Debug.LogError("buttonNextAction == null");
		}
	}

	IEnumerator LoadXMLResource() {
		string strPath = string.Empty;
		// 플랫폼 처리
		// #if ( UNITY_EDI)
		#if (UNITY_EDITOR || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN)
      		// Debug.Log("Unity Editor");
    	#endif

		#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS)
			Debug.Log("UNITY_EDITOR");
			strPath += ("file:///");
			strPath += (Application.streamingAssetsPath + "/" + m_strName);
		#elif UNITY_ANDROID
			strPath =  "jar:file://" + Application.dataPath + "!/assets/" + m_strName;
		#endif
		Debug.Log("strPath: " + strPath);
		WWW www = new WWW(strPath);

		yield return www;

		Debug.Log("Read Content: " + www.text.Length);

		StringReader stringReader = new StringReader(www.text);

		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(stringReader.ReadToEnd());
		// Interpret(www.text);
		progress();
	}
	private void printError(string msg) {
		Debug.Log(msg + " (XPath: "+gameProgress.getXPath()+")");
	}
	private void progress() {
		XmlNodeList xmlNodeList = null;
		Debug.Log("getXPath: "+ gameProgress.getXPath());
		xmlNodeList = xmlDoc.SelectNodes(gameProgress.getXPath());
		
		if (xmlNodeList.Count == 0) {
			printError("ERROR: xmlNodeList.Count == 0");
			return;
		}

		XmlNode actionNode = xmlNodeList[0];
		if (actionNode.Attributes["name"] == null) {
			printError("ERROR: actionNode.Attributes[\"name\"] is null");
			return;
		}
		BDAction.CompletionDelegate completion = delegate() {
			// Next action.
			Debug.Log("Next Action!!");
			gameProgress.action++;
			progress();
		};
		BDAction bdAction = BDAction.create(actionNode);
		bdAction.start(completion);
		// Pro ->
		// 자동으로 다음 액션으로 넘어가도 되는 것이 있지만, 그렇지 않은 액션이 있다.
		// 액션의 구분은 BDAction 내부에서 이루어진다.
		// delegate callback도 짜피 액션의 종류에 의해이루어진다.
		// chatbox와 같이 자동으로 넘어가면 안되는 액션에서 completion을 무시하면 되지않을까?
	}

}