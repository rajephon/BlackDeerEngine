﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Xml;
using System.Text;
using System;

public class BlackDeerEngine : MonoBehaviour {
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
	}
	
	// Update is called once per frame
	void Update () {
		
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
		Debug.Log("A c: " + xmlNodeList.Count);
		XmlNode actionNode = xmlNodeList[0];
		if (actionNode.Attributes["name"] == null) {
			printError("ERROR: actionNode.Attributes[\"name\"] is null");
			return;
		}
		BDAction bdAction = new BDAction(actionNode.Attributes["name"].Value);

		// foreach(XmlNode node in xmlNodeList) {
		// 	if (node.Name.Equals("stage") && node.HasChildNodes) {
		// 		foreach(XmlNode child in node.ChildNodes) {
		// 			Debug.Log("id: "+ child.Name);
		// 			if (child.Name == "episode") {
		// 			}
		// 			// Debug.Log("id: " + child.Attributes.GetNamedItem("id").Value);
		// 		}
		// 	}
		// }
	}

}