# XML Script

변동이 가능하여 간단하게 적어둡니다.

## Examples
```xml
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<stage value="1">
    <map name="설원">
        <cutscene no="1" isIntro="false">
                <action name="fadein" value="2"></action>
                <action name="chat" value="TestPlayer01" value2="메세지 테스트111"></action>
                <action name="chat" value="TestPlayer02" value2="메세지 테스트222"></action>
                <action name="chat" value="TestPlayer01" value2="메세지 테스트333"></action>
        </cutscene>
        <cutscene no="2" isIntro="false">
                <action name="chat" value="TestPlayer01" value2="메세지 테스트111"></action>
                <action name="chat" value="TestPlayer02" value2="메세지 테스트222"></action>
                <action name="timeline" value="Timeline01"></action>
                <action name="chat" value="TestPlayer02" value2="메세지 테스트333"></action>
                <action name="chat" value="TestPlayer01" value2="메세지 테스트444"></action>
        </cutscene>
    </map>
    <clear con="위치도달"></clear>
</stage>
```

## Structure
stage-map-cutscene-action 4단계로 되어있습니다.

- ### action
   동작의 최소단위입니다. 하나의 동작이 하나의 효과로 이루어져있습니다.

   하나의 action이 끝나면 다음 action으로 넘어가거나 사용자의 입력을 기다리기도 합니다.

   e.g. fadein, fadeout, cameraAnimation, chat, timeline

   ▶ [액션리스트](Document/ActionList.md)

   -  **name** : string / 액션의 이름
   
   -  **value** : string / 속성1
    
   -  **value2** : string / 속성2

- ### cutscene
   action의 묶음입니다. intro로 설정하여 특정 장소에 들어올 때 바동적으로 실행되기도 하며, 특정 조건이 도달하면 실행하도록 할 수 있습니다.

   scene을 실행하면 scene 내부의 action을 순차적으로 실행합니다.

   -  **no** : integer / 해당 컷씬의 번호
   
   -  **isIntro** : bool / true일 경우, 해당 맵 진입시, 혹은 BlackDeerEngine의 로드가 끝날 경우 자동 동작합니다.


- ### map
   특정 장소에 대한 cutscene의 묶음입니다. 특정 스테이지의 에피소드에서 해당 장소에서 이루어지는 cutscene을 포함합니다.

   -  **name** : string / 맵의 이름

- ### stage
   이야기에 대한 총 집합체입니다. map, 클리어 조건 등을 모두 포함합니다.

   -  **value** : ingeter / 해당 스테이지의 번호

- ### clear
스테이지 클리어 조건입니다. 여러개의 조건을 포함할 수 있습니다.

   -  **con** : 클리어 조건을 입력합니다.
      - **위치도달** : 특정 위치에 도달할 경우 클리어 콜백을 발생합니다.
