
## Action List

| 액션이름         | value      | value2         | value3 | 자동진행 | 짧은 설명                      |
|----------------|------------|----------------|--------|----------|---------------------------|
| fadein         | 시간        |                |        | O        | 화면 전체 페이드인             |
| fadeout        | 시간        |                |        | O        | 화면 전체 페이드아웃           |
| chat           | 화자        | 메세지           |        | X        | 메세지 출력.                |
| timeline       | 파일이름     |                |        | O        | 카메라 무빙 등 타임라인 동작 |

## Description

게임의 드라마틱한 연출을 위한 동작들입니다.

순차 진행 방식이며, 액션이 끝나면 자동으로 다음 액션이 수행되는 것과 그렇지 않은 액션이 있습니다.
'자동진행' 컬럼이 바로 그것에 대한 표시입니다.

- ### fadein
   화면 전체 페이드인 효과를 줍니다. value 항목으로 동작 시간을 정할 수 있습니다. 단위는 '초'입니다.

- ### fadeout
   화면 전체 페이드아웃 효과를 줍니다. value 항목으로 동작 시간을 정할 수 있습니다. 단위는 '초'입니다.
   
- ### chat
   대화 메세지를 출력합니다. value에는 화자를, value2에는 표시를 원하는 메세지를 입력합니다.
   
   value에 입력할 화자는 유니티 scene에 존재하는 오브젝트여야 합니다.
   
- ### timeline
   타임라인 애니메이션을 동작합니다. value에는 Resources/Timeline/ 디렉토리 내의 타임라인 파일명을 지정합니다.(※ 확장자 없이 입력합니다.)
   
   타임라인 생성 방법은 [Document/README.md#timeline](Document/README.md#timeline)을 참고바랍니다.

