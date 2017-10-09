using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System.Xml;
using System.Text;
using System;

public class BlackDeerEngine : BlackDeerSingleton<BlackDeerEngine> {
	protected BlackDeerEngine () {}
	private GameObject fadePanel = null;
	public GameObject playerObject;
	public Camera mainCamera = null;
	public Canvas mainCanvas = null;
	public Button buttonNextAction = null;

	public string scriptPath = "xml/SampleScript.xml";
	public GameObject destinationPos;

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

		public void setProgressStep(int stage, string map, int cutscene, int action) {
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
		
		private GameObject destinationObject;
		public GameObject DestinationObject {
			get {
				return destinationObject;
			}
			set {
				destinationObject = value;
			}
		}

		public bool hasDestination(XmlDocument xmlDoc) {
			if (xmlDoc == null) {
				Debug.Log("XmlDoc is null");
				return false;
			}
			XmlNode clearNode = xmlDoc.SelectSingleNode("stage[@value=\""+stage+"\"]/clear[@con=\"위치도달\"]");
			if (clearNode == null) {
				return false;
			}
			return true;
		}
	}

	private GameProgress gameProgress;
	private XmlDocument xmlDoc;
	private bool isProgressing = false;

	// Delegate
	public delegate void StageClearDelegate(GameProgress gameProgress);
	public delegate void BlockUserControlDelegate(bool isBlocked);
	public delegate void LoadCompleteDelegate();
	private StageClearDelegate delegateStageClear;
	public StageClearDelegate DelegateStageClear {
		get {
			return delegateStageClear;
		}
		set {
			delegateStageClear = value;
		}
	}
	private BlockUserControlDelegate delegateBlockUserControl;
	public BlockUserControlDelegate DelegateBlockUserControl {
		get {
			return delegateBlockUserControl;
		}
		set {
			delegateBlockUserControl = value;
		}
	}
	private LoadCompleteDelegate loadCompleteDelegate;
	public LoadCompleteDelegate DelegateLoadComplete {
		get {
			return loadCompleteDelegate;
		}
		set {
			loadCompleteDelegate = value;
		}
	}
	public void setProgressStep(int stage, string map, int cutscene, int action) {
		gameProgress.setProgressStep(stage, map, cutscene, action);
	}
	// Use this for initialization
	void Start () {
		gameProgress = new GameProgress(1, "설원", 1, 1);
		StartCoroutine(StartRutine());
	}
	
	// Update is called once per frame
	void Update () {
		if (playerObject != null) {
			if (gameProgress.DestinationObject != null) {
				Vector3 playerPos = playerObject.transform.position;
				Vector3 destPos = gameProgress.DestinationObject.transform.position;
				float dist = Vector3.Distance(playerPos, destPos);
				if (dist <= 0.7f) {
					// TODO: 스테이지 클리어 처리
					if (delegateStageClear != null) {
						delegateStageClear(gameProgress);
					}
					gameProgress.DestinationObject = null;
					Debug.Log("STAGE CLEAR");
				}
			}
		}
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
		
		if (gameProgress.hasDestination(xmlDoc)) {
			Debug.Log("위치도달 조건이 있습니다.");
			if (destinationPos == null) {
				Debug.LogWarning("위치도달 포지션을 세팅해야 합니다..!!");
			}
			gameProgress.DestinationObject = destinationPos;
		}else {
			Debug.Log("위치도달 조건이 없습니다.");
		}
	}

	IEnumerator StartRutine() {
		string strPath = string.Empty;
		// 플랫폼 처리
		// #if ( UNITY_EDI)
		#if (UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_IOS)
			Debug.Log("UNITY_EDITOR");
			strPath += ("file://");
			strPath += (Application.streamingAssetsPath + "/" + scriptPath);
		#elif UNITY_ANDROID
			strPath =  "jar:file://" + Application.dataPath + "!/assets/" + m_strName;
		#endif

		Debug.Log("strPath: " + strPath);
		
		if (File.Exists(strPath)) {
			Debug.Log("Exists true");
		}else {
			Debug.Log("Exists false");
		}

		WWW www = new WWW(strPath);
		while(!www.isDone)
			yield return www;

		Debug.Log("Read Content: " + www.text.Length);
		StringReader stringReader = new StringReader(www.text);

		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(stringReader.ReadToEnd());

		initEnvironment();
		// if (gameProgress.isIntro(xmlDoc)) {
			// startProgress();
		// }
		if (loadCompleteDelegate != null) {
			loadCompleteDelegate();
		}
	}

	public void startProgress() {
		isProgressing = true;
		progress();
	}
	public void stopProgress() {
		isProgressing = false;
		// TODO: remove progress 
	}
	private void printError(string msg) {
		Debug.Log(msg + " (XPath: "+gameProgress.getXPath()+")");
	}
	private void progress() {
		// TODO: 플레이어 움직임 조작 막기.
		if (delegateBlockUserControl != null) {
			delegateBlockUserControl(true);
		}
		XmlNode actionNode = null;
		Debug.Log("getXPath: "+ gameProgress.getXPath());
		actionNode = xmlDoc.SelectSingleNode(gameProgress.getXPath());

		if (actionNode == null) {
			printError("ActionNode == null");
			// TODO: 플레이어 움직임 조작 가능하게.
			if (delegateBlockUserControl != null) {
				delegateBlockUserControl(false);
			}
			return;
		}

		if (actionNode.Attributes["name"] == null) {
			printError("ERROR: actionNode.Attributes[\"name\"] is null");
			return;
		}
		BDAction.CompletionDelegate completion = delegate() {
			Debug.Log("Next Action!!");
			gameProgress.action++;
			if (isProgressing) {
				progress();
			}
		};
		BDAction bdAction = BDAction.create(actionNode);
		bdAction.start(completion);
	}
}