
# HOW TO USE

## 1. Download & Import

자신의 프로젝트에 **BlackDeerEngine.unitypackage**를 임포트합니다.

   * 패키지는 repo의 Package/ 디렉토리 안에 있습니다.
   
   * 만약 필수요소만 임포트하고 싶다면 Examples, StreamingAssets, Resources 디렉토리는 생략해도 됩니다.

## 2. DeerBlackEngine 세팅

1. GameObject - Create Empty로 빈 프로젝트를 생성
2. BlackDeerEngine.cs를 Add Component
3. public variable 세팅

<p align="center">
  <img src="https://github.com/rajephon/BlackDeerEngine/blob/master/Document/BlackDeerEngineComponent.png" width="299" />
</p>

**Player Object** : Scene내 플레이어의 Object를 연결합니다. 위치 도달 등을 확인할 때 사용됩니다.

**Main Camera** : 이름 그대로 메인 카메라 연결

**Main Canvas** : UI를 담고 있는 Canvas Object 연결

**Button Next Action** : 대화가 표시된 다음 터치하여 다음으로 넘길 Button Object 연결

**Script Path** : StreamingAssets 내부에 있는 스크립트 경로 입력

**Destination Pos** : XML스크립트의 스테이지 클리어 조건에 위치도달이 있을 경우, 도달여부를 체크할 도착지 위치의 GameObject를 연결

**Timeline Object** : [Timeline](#timeline)을 관리하는 GameObject를 연결

------

## Delegate

### Define
```C#
public delegate void StageClearDelegate(GameProgress gameProgress);
public delegate void BlockUserControlDelegate(bool isBlocked);
```

### public delegate void LoadCompleteDelegate();
BlackDeerEngine의 초기화가 모두 끝날 때 불리는 콜백입니다.

### public delegate void StageClearDelegate(GameProgress gameProgress);
스테이지 클리어조건 달성 시 불리는 콜백입니다.

### public delegate void BlockUserControlDelegate(bool isBlocked);
Action 동작 시작으로 사용자의 조작이 막혀야 하는 상황인지 아닌지에 대한 콜백입니다.

기본적으로 cutscene이 시작할 때 true, 끝날 때 false 콜백이 옵니다.

### Example
```C#
BlackDeerEngine.Instance.DelegateStageClear = delegate(BlackDeerEngine.GameProgress gameProgress) {
	Debug.Log(gameProgress.stage+ " STAGE CLEAR");
};
BlackDeerEngine.Instance.DelegateBlockUserControl = delegate(bool uiblocked) {
	 Debug.Log("UIBlockedMode: "+uiblocked);
};
BlackDeerEngine.Instance.DelegateLoadComplete = delegate() {
	BlackDeerEngine.Instance.setProgressStep(1, "설원", 2, 1);
	Debug.Log("Load Complete");
	BlackDeerEngine.Instance.startProgress();
};
```
------

## Timeline
역동적인 컷신을 만들기 위해 활용합니다. 

### Timeline Object 생성
1. Menu - GameObject - Create Empty 로 빈 오브젝트를 생성합니다. 이름은 임의로 정하셔도 됩니다.
2. 해당 오브젝트 Component에 **Playable Director**와 **Animator**를 추가합니다.

### Timeline 만들기
1. Timeline Object가 클릭된 채로 Timeline Tab을 선택합니다.
2. Playable옵션에 특별히 지정된 Timeline이 없을 경우, **Create**버튼이 활성화되어있습니다. 해당 버튼을 클릭하여 생성합니다. 생성되는 Timeline 파일의 위치는 Resources/Timeline/ 디렉토리 안으로 합니다.

------


