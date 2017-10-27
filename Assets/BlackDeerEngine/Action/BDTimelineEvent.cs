using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BDTimelineEvent : PlayableAsset {
	TestPlayableBehaviour behaviour;
	public BDTimelineEvent() {
		behaviour = new TestPlayableBehaviour ();
	}
	public override Playable CreatePlayable(PlayableGraph  graph , GameObject  go) {
		return ScriptPlayable <TestPlayableBehaviour> .Create (graph, behaviour);
	}
	public void setCompletionDelegate(BDAction.CompletionDelegate completionDelegate) {
		behaviour.setCompletionDelegate(completionDelegate);
	}
}

public  class  TestPlayableBehaviour : PlayableBehaviour {
	private BDAction.CompletionDelegate completionDelegate = null;
	public void setCompletionDelegate(BDAction.CompletionDelegate completionDelegate) {
		this.completionDelegate = completionDelegate;
	}
	public  override  void  OnGraphStart ( Playable  playable ) {
	}
	// 타임 라인 정지시 실행
	public  override  void  OnGraphStop ( Playable  playable ) {
		Debug.Log("OnGraphStop");
		if (completionDelegate != null) {
			completionDelegate();
		}
	}
	// PlayableTrack 재생시 실행
	public  override  void  OnBehaviourPlay ( Playable  playable , FrameData  info ) {
	}
	// PlayableTrack 정지시 실행
	public  override  void  OnBehaviourPause ( Playable  playable , FrameData  info ) {
	}
	// PlayableTrack 재생시 매 프레임 실행
	public  override  void  PrepareFrame ( Playable  playable , FrameData  info ) {
		
	}
}