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
```
* **Game Title** : 게임 실행, 게임 데이터 불러오기, 게임 종료의 기능을 담당하는 씬이다.
- Start : 게임을 새로 실행한다.
- Load :  직전에 저장한 정보를 불러와 게임을 실행한다.
- Exit : 게임을 종료한다.
* **Game Stage** : 사냥, 건축, 수영, 제작 등의 기능을 담당하는 본 게임 씬이다.
- Save : 게임 정보를 저장한다.
- Load : 직전에 저장한 정보를 불러와 게임을 실행한다.
- Exit : Game Title로 나간다.
```

## 2.3. ERD
![ERD1](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/ERD1.png)
```
* 프랜차이즈 서비스에 대한 물리적 ERD이다.
```

![ERD2](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/ERD2.png)
```
* member(회원 정보) 테이블의 ID 애트리뷰트를 Bookmark(즐겨찾기) 테이블의 외래키로 설정한다.
  각 문의사항에는 하나의 답변만이 가능하므로 1:1 관계이다.
* member(회원 정보) 테이블의 ID 애트리뷰트를 survey(설문조사 결과) 테이블의 외래키로 설정한다.
  각 회원의 설문조사 결과는 하나이므로 1:1관계이다.
```

****
# 3. 제공 기능
## 3.1. 핵심 기능
* **메인 화면**
![메인](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/메인.png)
```
설명 : 사용자는 좌측 상단의 메뉴 버튼을 통해 부가 기능을 이용할 수 있다.
```

* **상권 분석**
![상권분석](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/분석.png)
```
설명 : 사용자는 설정한 범위 내의 상권 관련 데이터를 출력한다.
출력 데이터 : 상권 개요, 매출 분석, 인구 분석, 점포 현황
```

* **상권 추천**
![상권추천](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/추천.png)
```
설명 : 사용자는 실시한 설문조사 결과와 분석한 데이터를 비교하고, 상권을 추천받는다.
출력 데이터 : 상권 개요, 매출 분석, 인구 분석, 점포 현황, 상세평가지수
```

* **프랜차이즈**
![프랜차이즈](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/프랜차이즈.png)
```
설명 : 사용자는 원하는 프랜차이즈의 상세정보를 출력한다.
출력 데이터 : 연 평균 매출액, 초기 부담금, 초기 인테리어 비용, 가맹점 수, 광고비, 판촉비, 가맹 계약기간
```

## 3.2. 부가 기능
* **로그인 / ID,PW 찾기 / 회원 가입**
![로그인](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/로그인.png)
```
설명
- 사용자는 아이디, 비밀번호, 이름, 이메일 주소, 핸드폰 번호, 비밀번호 찾기 질문 입력을 통해 회원 가입을 진행한다.
- 회원가입 한 아이디, 비밀번호 입력을 통해 로그인이 가능하다.
- 회원가입 시 입력한 이름, 핸드폰 번호, 이메일 주소를 이용하여 ID 찾기가 가능하다.
- ID, PW 질문 & 답변을 통해 PW 찾기가 가능하다.
```

* **마이페이지**
![마이페이지](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/마이페이지.png)
```
설명 : 사용자는 회원 가입 시 작성한 개인 정보를 조회 및 수정이 가능하다.
```

* **공지 사항**
![공지사항](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/공지.png)
```
설명 : 사용자는 관리자가 게시한 창업 관련 프로그램, 업데이트 예정 등의 공지사항 조회가 가능하다.
```

* **문의 사항**
![문의사항](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/문의.png)
```
설명 : 사용자는 문의사항을 작성할 수 있다.
```

* **설문 조사**
![설문 조사](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/기록.png)
```
설명 : 사용자는 이전에 시행했던 설문 조사 기록 및 내용을 조회할 수 있다.
```

* **관심 목록**
![관심목록](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/관심목록.png)
```
설명 : 로그인 한 사용자는 상권 분석 기능 이용 중에 관심있는 지역을 저장할 수 있다.
```

* **광고 배너 / 핫이슈**
![광고](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/광고.png)
```
설명 : 상권 관련 광고를 제공해주는 광고 배너 기능과, 최근 이슈인 뉴스들을 제공해주는 핫이슈 기능이 있다.
```

****
# 4. 사용 기술
## 4.1. 추천 알고리즘 - ex) 성장성
![성장성1](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/성장성1.png)
```
설명 : 각 double형 변수에 해당 값을 저장하고, 이를 이용해 수식을 계산한다.
```
![성장성2](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/성장성2.png)
```
설명 : 성장성(매출 규모 증감률)에 관한 증감률을 계산, 성장성(매출 규모 증감률)에 관한 최대값 반환
```
![성장성3](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/성장성3.png)
```
설명 : 성장성(매출 규모 증감률)에 관한 최소값 반환
```

## 4.2. 데이터 크롤링
![크롤링1](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/크롤링1.png)
```
설명 : BASE_URL 설정 및 파일 저장 형식을 설정한다.
```
![크롤링2](https://github.com/Jeongwonseok/Portfolio_JWS/blob/master/image/크롤링2.png)
```
설명 : 상호, 영업표지, 대표자, 업종, 사업자등록일, 대표번호 등 총 13가지 종류의 프랜차이즈 정보를 출력한다.
```

****
# 5. 부록
## 5.1. 수상 이력
* 창의적 공학설계 경진대회 은상 (성결대학교)
* 컴퓨터공학과 제 21회 졸업작품 전시회 우수상 (성결대학교)
* 2019 컴퓨터공학과 경진대회 설계프로젝트 최우수상 (성결대학교)
