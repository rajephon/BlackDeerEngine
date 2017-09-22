using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
			string cameraName = actionNode.Attributes["value"].Value;
			string animName = actionNode.Attributes["value2"].Value;
			if (cameraName == null || animName == null) {
				Debug.Log("cameraName == null || animName == null");
			}
			return new BDActionCameraMove(cameraName, animName);
		}else if (actionName == "대화") {
			string chatMessage = actionNode.Attributes["value"].Value;
			if (chatMessage == null) {
				Debug.Log("chatMessage == null");
			}
			return new BDActionChatMessage(chatMessage);
		}else if (actionName == "대화보이기") {
			return new BDActionChatEnable(true);
		}else if (actionName == "대화감추기") {
			return new BDActionChatEnable(false);
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

public class BDActionChatEnable: BDAction {
	private static Text txtChatbox = null;
	private static BDNextActionButton nextActionButton = null;
	private bool isVisible = true;
	public BDActionChatEnable(bool isVisible) {
		this.isVisible = isVisible;
	}
	public static void setTxtChatbox(Text txtChatbox, BDNextActionButton nextActionButton, bool isVisible) {
		setTxtChatbox(txtChatbox, nextActionButton);
		txtChatbox.enabled = isVisible;
		nextActionButton.setHidden(isVisible);
	}
	public static void setTxtChatbox(Text txtChatbox, BDNextActionButton nextActionButton) {
		BDActionChatEnable.txtChatbox = txtChatbox;
		BDActionChatEnable.nextActionButton = nextActionButton;
	}
	public override void start(CompletionDelegate completionDelegate) {
		txtChatbox.enabled = isVisible;
		nextActionButton.setHidden(isVisible);

		if (completionDelegate != null) {
			completionDelegate();
		}
	}
}

public class BDActionChatMessage: BDAction {
	private static Text txtChatbox = null;
	private static BDNextActionButton nextActionButton = null;
	private string chatMessage = "";
	public static void setTxtChatbox(Text txtChatbox, BDNextActionButton nextActionButton) {
		BDActionChatMessage.txtChatbox = txtChatbox;
		BDActionChatMessage.nextActionButton = nextActionButton;
	}
	public BDActionChatMessage(string chatMessage) {
		if (chatMessage != null) {
			this.chatMessage = chatMessage;
		}
	}
	public override void start(CompletionDelegate completionDelegate) {
		if (txtChatbox == null || chatMessage == null) {
			return;
		}
		nextActionButton.setCompletionDelegate(completionDelegate);
		nextActionButton.setEnabled(true);
		txtChatbox.text = chatMessage;
	}
}

public class BDActionCameraMove: BDAction {
	private static Animator animator = null;
	private static Camera camera = null;
	private string cameraName = null;
	private string animName = null;
	public static void setMainCamera(Camera camera) {
		// BDActionCameraMove.camera = camera;
	}
	public BDActionCameraMove(string cameraName, string animName) {
		if (cameraName == "메인카메라") {
			camera = Camera.main;
		}

		if (camera != null) {
			this.cameraName = cameraName;
			this.animName = animName;

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
		if (camera == null || animator == null || animName == null) {
			Debug.Log("camera == null || animator == null || animName == null");
			return;
		}
		BDCameraAnimCallback callback = camera.gameObject.GetComponent<BDCameraAnimCallback>();
		if (callback == null) {
			Debug.Log("BDCameraAnimCallback == null");
			return;
		}
		callback.setCompletionDelegate(completionDelegate);
		animator.Play(animName, 0);
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

