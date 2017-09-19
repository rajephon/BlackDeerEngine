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
	public delegate void CompletionDelegate();
	public static BDAction create(XmlNode actionNode) {
		string actionName = actionNode.Attributes["name"].Value;
		Debug.Log("next action : " + actionName);
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
		}else if (actionName == "카메라애니메이션") {
			return new BDActionCameraMove();
		}
		return null;
	}
	public virtual void start(CompletionDelegate completionDelegate) {
		Debug.Log("부모 클래스의 start()가 호출되었습니다. action name : "+name);
		completionDelegate();
	}
	public string Name {
		get { return name; }
		set { this.name = Name; }
	}

}

public class BDActionCameraMove: BDAction {
	private static Animator animator = null;
	private static Camera camera = null;
	public static void setMainCamera(Camera camera) {
		BDActionCameraMove.camera = camera;
	}
	public BDActionCameraMove() {
		if (camera != null) {
			Animator currAnimator = camera.GetComponent<Animator>();
			if (currAnimator == null) {
				currAnimator = camera.gameObject.AddComponent<Animator>();
			}
			RuntimeAnimatorController animatorController = Resources.Load<RuntimeAnimatorController>("CameraAnimation/MainCamera");
//			currAnimator.runtimeAnimatorController = Resources.Load("Assets/BlackDeerEngine/CameraAnimation/Main Camera.controller") as RuntimeAnimatorController;
			if (animatorController == null) {
				Debug.Log("runtime animator load error");
			}else {
				currAnimator.runtimeAnimatorController = animatorController;
			}
			animator = currAnimator;
		}
	}

	public override void start(CompletionDelegate completionDelegate) {
		if (camera == null || animator == null) {
			Debug.Log("camera == null || animator == null");
			return;
		}
		animator.Play("sample00", 0);
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
	public override void start(CompletionDelegate completionDelegate) {
		Debug.Log("BDActionFade"+((isFadeIn)?"In":"Out"));
		if (bdFadeInOut == null) {
			Debug.Log("- bdFadeInOut component is null");
			return;
		}
		if (isFadeIn) {
			bdFadeInOut.startFadeIn(period, completionDelegate);
		}else {
			bdFadeInOut.startFadeOut(period, completionDelegate);
		}
	}
}

