﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.Xml;

public class BDAction
{
    private string name = "";
    public BDAction()
    {

    }
    public BDAction(string name)
    {
        this.name = name;
    }
    public delegate void CompletionDelegate();
    public static BDAction create(XmlNode actionNode)
    {
        string actionName = actionNode.Attributes["name"].Value;
        Debug.Log("next action : " + actionName);
        if (actionName == "fadeout" || actionName == "fadein")
        {
            float period = 2.0f;
            if (actionNode.Attributes["value"] != null)
            {
                period = float.Parse(actionNode.Attributes["value"].Value, System.Globalization.CultureInfo.InvariantCulture);
            }
            if (actionName == "fadein")
            {
                return new BDActionFadeInOut(period, true);
            }
            else
            {
                return new BDActionFadeInOut(period, false);
            }
        }
        else if (actionName == "chat")
        {
            string speakerName = actionNode.Attributes["value"].Value;
            if (speakerName == null)
                Debug.Log("speakerName == null");
            string chatMessage = actionNode.Attributes["value2"].Value;
            if (chatMessage == null)
                Debug.Log("chatMessage == null");
            return new BDActionChatMessage(speakerName, chatMessage);
        }
        else if (actionName == "timeline")
        {
            Debug.Log("Timeline");
            string timelineFiles = actionNode.Attributes["value"].Value;
            if (timelineFiles == null)
                Debug.Log("timelineFiles is null");
            return new BDActionTimeLine(timelineFiles);
        }else if (actionName == "SoundEffect") {
            Debug.Log("SoundEffect");
            string soundPath = actionNode.Attributes["value"].Value;
            if (soundPath == null)
                Debug.Log("SoundPath is null");
            return new BDActionSoundEffect(soundPath);
        }
        return null;
    }
    public virtual void start(CompletionDelegate completionDelegate)
    {
        Debug.Log("부모 클래스의 start()가 호출되었습니다. action name : " + name);
        completionDelegate();
    }
    public string Name
    {
        get { return name; }
        set { this.name = Name; }
    }

}

public class BDActionSoundEffect : BDAction {
    private string path = "";
    private static AudioSource audioSource;
    public BDActionSoundEffect(string path) {
        this.path = path;
    }
    public static void setAudioSource(AudioSource audioSource) {
        BDActionSoundEffect.audioSource = audioSource;
    }
    public override void start(CompletionDelegate completionDelegate) {
        if (audioSource != null) {
            audioSource.PlayOneShot((AudioClip)Resources.Load(path));
        }
        completionDelegate();
    }
}

public class BDActionTimeLine : BDAction
{
    private static PlayableDirector playableDirector;
	private TimelineAsset timelineAsset;
    public static void setPlayableDirector(GameObject timelineObject)
    {
        playableDirector = timelineObject.GetComponent<PlayableDirector>();
    }
	private void deleteCallbackTrack(TimelineAsset timelineAsset) {
		List<TrackAsset> ctrack = new List<TrackAsset>();
		foreach(TrackAsset track in timelineAsset.GetRootTracks()) {
			if (track.name == "callback_track") {
				ctrack.Add(track);
			}
		}
		foreach(TrackAsset track in ctrack) {
			timelineAsset.DeleteTrack(track);
		}
	}
    public BDActionTimeLine(string timelineName) {
        // playable
        this.timelineAsset = Resources.Load<TimelineAsset>("Timeline/"+timelineName);
        if (timelineAsset == null) {
            Debug.Log("timelineAsset load error");
        }else{
			this.deleteCallbackTrack(timelineAsset);
			
        }
    }
    public override void start(CompletionDelegate completionDelegate) {
        PlayableTrack ctrack = timelineAsset.CreateTrack<PlayableTrack>(null, "callback_track");
        TimelineClip tclip = ctrack.CreateClip<BDTimelineEvent>();
        BDTimelineEvent timelineEvent = ScriptableObject.CreateInstance<BDTimelineEvent>();
        timelineEvent.setCompletionDelegate(completionDelegate);
        tclip.asset = timelineEvent;
        double duration = timelineAsset.duration;
        tclip.start = duration - 0.1f;
        tclip.duration = 0;
        tclip.displayName = "eventCallback";
		playableDirector.Play(timelineAsset);
    }
}

public class BDActionChatMessage : BDAction
{
    private static GameObject txtChatbox = null;
    private static BDNextActionButton nextActionButton = null;
    private string chatMessage = "";
    public static void setTxtChatbox(BDNextActionButton nextActionButton, Canvas canvas)
    {
        BDActionChatMessage.nextActionButton = nextActionButton;
        BDChatMessage.setCanvas(canvas);
    }
    public BDActionChatMessage(string speakerName, string chatMessage)
    {
        // GameObject speaker = GameObject.Find(speaker);
        GameObject speaker = GameObject.Find(speakerName);
        if (speaker != null)
        {
            // TODO: add label with text
            txtChatbox = BDChatMessage.createChatbubble(speaker, chatMessage);
        }
        else
        {
            Debug.Log("speaker is null");
        }
        if (chatMessage != null)
        {
            this.chatMessage = chatMessage;
        }
    }
    public override void start(CompletionDelegate completionDelegate)
    {
        if (txtChatbox == null || chatMessage == null)
        {
            return;
        }
        txtChatbox.GetComponent<Text>().enabled = true;
        nextActionButton.setCompletionDelegate(delegate ()
        {
            if (txtChatbox != null)
            {
                Object.Destroy(txtChatbox.gameObject);
            }
            completionDelegate();
        });
        nextActionButton.setHidden(false);
        nextActionButton.setEnabled(true);
    }
}

public class BDActionFadeInOut : BDAction
{
    private static BDFadeInOut bdFadeInOut = null;
    public static void setFadePanel(BDFadeInOut fadePanel)
    {
        BDActionFadeInOut.bdFadeInOut = fadePanel;
    }
    private float period = 0.1f;
    private bool isFadeIn = true;
    public BDActionFadeInOut(float period, bool isFadeIn)
    {
        this.period = period;
        this.isFadeIn = isFadeIn;
    }
    public override void start(CompletionDelegate completionDelegate)
    {
        Debug.Log("BDActionFade" + ((isFadeIn) ? "In" : "Out"));
        if (bdFadeInOut == null)
        {
            Debug.Log("- bdFadeInOut component is null");
            return;
        }
        if (isFadeIn)
        {
            bdFadeInOut.startFadeIn(period, completionDelegate);
        }
        else
        {
            bdFadeInOut.startFadeOut(period, completionDelegate);
        }
    }
}

