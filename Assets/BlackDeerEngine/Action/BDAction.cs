using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class BDAction {
	private string name = "";
	public BDAction() {

	}
	public BDAction(string name) {
		this.name = name;
	}

	public static BDAction create(XmlNode actionNode) {
		string actionName = actionNode.Attributes["name"].Value;
		if (actionName == "페이드아웃" || actionName == "페이드인") {
			float period = 2.0f;
			if (actionNode.Attributes["value"] != null) {
				period = float.Parse(actionNode.Attributes["value"].Value, System.Globalization.CultureInfo.InvariantCulture);
			}
			if (actionName == "페이드인") {
				return new BDActionFadeInOut(period, true);
			}else {
				return new BDActionFadeInOut(period, false);
			}
		}
		return null;
	}
	public virtual void start() {
		Debug.Log("부모 클래스의 start()가 호출되었습니다. action name : "+name);
	}
	public string Name {
		get { return name; }
		set { this.name = Name; }
	}

}

public class BDActionFadeInOut: BDAction {
	private static BDFadeInOut bdFadeInOut = null;
	public static void setFadePanel(BDFadeInOut fadePanel) {
		BDActionFadeInOut.bdFadeInOut = fadePanel;
	}
	private float period = 0.1f;
	private bool isFadeIn = true;
	public BDActionFadeInOut(float period, bool isFadeIn) {
		this.period = period;
		this.isFadeIn = isFadeIn;
	}
	public override void start() {
		Debug.Log("BDActionFade"+((isFadeIn)?"In":"Out"));
		if (bdFadeInOut == null) {
			Debug.Log("- bdFadeInOut component is null");
			return;
		}
		if (isFadeIn) {
			bdFadeInOut.startFadeIn(period);
		}else {
			bdFadeInOut.startFadeOut(period);
		}
	}
}

