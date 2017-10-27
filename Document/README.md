
# HOW TO USE

## 1. Download & Import

**BlackDeerEngine.unitypackage**를 임포트합니다.
(만약 필수요소만 임포트하고 싶다면 Examples, StreamingAssets, Resources 디렉토리는 생략해도 됩니다.)

## 2. DeerBlackEngine 세팅

- GameObject - Create Empty로 빈 프로젝트를 생성
- BlackDeerEngine.cs를 Add Component
- public variable 세팅

<p align="center">
  <img src="https://github.com/rajephon/BlackDeerEngine/blob/document/Document/BlackDeerEngineComponent.png" width="299" />
</p>

**Player Object** : Scene내 플레이어의 Object를 연결합니다. 위치 도달 등을 확인할 때 사용됩니다.

**Main Camera** : 이름 그대로 메인 카메라 연결

**Main Canvas** : UI를 담고 있는 Canvas Object 연결

**Button Next Action** : 대화가 표시된 다음 터치하여 다음으로 넘길 Button Object 연결

**Script Path** : StreamingAssets 내부에 있는 스크립트 경로 입력

**Destination Pos** : XML스크립트의 스테이지 클리어 조건에 위치도달이 있을 경우, 도달여부를 체크할 도착지 위치의 GameObject를 연결

**Timeline Object** : Timeline을 관리하는 GameObject를 연결

------


## Timeline
