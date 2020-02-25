SurvivalGame (Unity3D)
======================

# 1. 개요
## 1.1. 주제
3D FPS 서바이벌 생존 게임 제작 (Unity3D)

## 1.2. 목적
Unity 엔진을 이용해서 1인칭 시점의 FPS 서바이벌 생존 게임을 제작한다.

## 1.3. 개발 범위
* Player - 게임 사용자가 직접 제어하는 캐릭터의 체력, 마력, 방어력, 공격력 등의 캐릭터와 관련된 정보들을 포함하며, 캐릭터의 움직임을 제어하도록 구현한다.
* UI - 화면에 표시되어야 하는 플레이어 정보, 무기 관련 정보를 구현한다.
* Building - 장애물, 모닥불, 연금 테이블 건축을 구현한다.
* NPC - 플레이어를 공격하는 동물, 플레이어에게서 도망치는 동물을 구현한다.
* Environment - 게임의 맵과 관련된 지형, 물, 낮과 밤 등을 구현한다.

## 1.4. 개발 환경
* Unity 2019.2.9f1 Personal
* Visual Studio 2020

****
# 2. 구조 설계
## 2.1. 클래스 구조도
![클래스](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/클래스.png)

## 2.2. 씬 구조도
![씬](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/씬.png)
* **Game Title** : 게임 실행, 게임 데이터 불러오기, 게임 종료의 기능을 담당하는 씬이다.
```
- Start : 게임을 새로 실행한다.
- Load :  직전에 저장한 정보를 불러와 게임을 실행한다.
- Exit : 게임을 종료한다.
```

* **Game Stage** : 사냥, 건축, 수영, 제작 등의 기능을 담당하는 본 게임 씬이다.
```
- Save : 게임 정보를 저장한다.
- Load : 직전에 저장한 정보를 불러와 게임을 실행한다.
- Exit : Game Title로 나간다.
```

****
# 3. 주요 구현부
## 3.1. AI
* **Nav Mesh Agent**<br>
<img src="https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/nav1.png" width="300" height="400"><br>
```
설명 : Nav Mesh Agent 컴포넌트 추가하고, 해당 옵션 값을 수정한다.
```

![nav2](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/nav2.png)
```
설명 : Nav Mesh Agent 변수를 선언하고, 생성자를 정의한다.
```

![nav3](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/nav3.png)
![nav4](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/nav4.png)
```
설명
- Unity Engine에서 자체적으로 제공하는 기능을 이용해 간단한 AI 동물을 구현한다.
- Player의 현재 위치를 매개변수로 받아, 경로 길이를 계산한다.
- 계산한 총 경로 길이를 반환하여 돼지 주변의 뛰고있는 Player의 움직임을 파악한다.
```

## 3.2. Singleton
* **SoundManager**
![single](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/single.png)
```
설명
- 게임에서는 수많은 Sound를 사용하는데, Sound들을 개별적으로 관리하는 것은 효율적이지 않다.
- 따라서, 효과적인 SoundManager 구현을 위해 Singleton을 구현한다.
- 게임 내부의 어느 곳에서든지 GameManager.instance 로 모두 접근이 가능하다.
- 하나의 객체로 중복 생성 없이 객체를 유지할 수 있다.
```

****
# 4. 참고
## 4.1. 스크린 샷
![ref1](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/ref1.png)




![ref2](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/FPS/ref2.png)

## 4.2. 참고 url
* 케이디 [유니티 3D 강좌] FPS 서바이벌 디펜스 : <https://www.youtube.com/watch?v=uandR5M30ho/>
