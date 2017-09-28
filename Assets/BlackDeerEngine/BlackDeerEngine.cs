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
	private GameObject fadePanel = null;
	public Camera mainCamera = null;
	// public Text txtChatbox = null;
	public Button buttonNextAction = null;

	public Canvas mainCanvas = null;
	public class GameProgress {
		public int stage;
		public string map;
		public int cutscene;
		public int action;
		public GameProgress(int stage, string map, int cutscene, int action) {
			this.stage = stage;
			this.map = map;
			this.cutscene = cutscene;
			this.action = action;
		}
		public string getXPath() {
			return getXPath(stage, map, cutscene, action);
		}
		public string getXPath(int stage, string map, int cutscene, int action) {
			return "stage[@value=\""+stage+"\"]/map[@name=\""+map+"\"]/cutscene[@no=\""+cutscene+"\"]/action["+action+"]";
		}
		public bool isIntro(XmlDocument xmlDoc) {
			if (xmlDoc == null) {
				Debug.Log("XmlDoc is null");
				return false;
			}
			XmlNode cutsceneNode = xmlDoc.SelectSingleNode("stage[@value=\""+stage+"\"]/map[@name=\""+map+"\"]/cutscene[@no=\""+cutscene+"\"]");
			if (cutsceneNode == null) {
				Debug.Log("CutsceneNode is null");
				return false;
			}
			if (cutsceneNode.Attributes["isIntro"] == null) {
				return false;
			}
			string isIntro = cutsceneNode.Attributes["isIntro"].Value;
			if (isIntro == "true")
				return true;
			return false;
		}
	}
	public string scriptPath = "xml/SampleScript.xml";
	private static GameProgress gameProgress;
	private XmlDocument xmlDoc;
	// Use this for initialization
	void Start () {
		gameProgress = new GameProgress(1, "설원", 1, 1);
		initEnvironment();
		StartCoroutine(LoadXMLResource());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void initEnvironment() {
		if (mainCanvas != null) {
			RectTransform canvasRect = mainCanvas.gameObject.GetComponent<RectTransform>();

			// create fadePanel
			GameObject fadePanel = new GameObject();
			fadePanel.name = "BDFadePanel";
			fadePanel.AddComponent<Image>();
			fadePanel.transform.SetParent(mainCanvas.transform);
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

		mainCamera.gameObject.AddComponent<BDCameraAnimCallback>();

		if (buttonNextAction != null) {
			BDNextActionButton nextActionButton = buttonNextAction.gameObject.AddComponent<BDNextActionButton>();
			buttonNextAction.onClick.AddListener(nextActionButton.onClicked);
			if (mainCanvas != null) {
				BDActionChatMessage.setTxtChatbox(nextActionButton, mainCanvas);
			}
			BDActionChatEnable.setTxtChatbox(nextActionButton, false);
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
			strPath += (Application.streamingAssetsPath + "/" + scriptPath);
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
		
		if (gameProgress.isIntro(xmlDoc)) {
			progress();
		}
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
		
	}

}