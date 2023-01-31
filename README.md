# K-Night
  판도라큐브 게임 개발 동아리 꼬꼬마 30일 프로젝트입니다.
  
  프로젝트의 이름은 K-Night로, King Night를 의미합니다.
  
  플레이어는 마을의 견습생으로, 마을에 괴물이 나타나는 비밀을 풀고 왕이 되는 게임입니다.
 
## 목차
  1. 게임 정보
  2. 기술 스택
  3. 구현 기능
  4. 배운 점
  
## 게임 정보
  - 플랫폼 : PC
  - 장르 : RPG
  - 사이드뷰

## 기술 스택
  - 게임 엔진 : Unity (2020.3.12f1 LTS)
  - 프로그래밍 언어 : C#

## 구현 기능
  - 캐릭터
    - 이동, 공격, 대쉬, 점프, 이단 점프
    - 체력, 공격력
  
  - 타이틀 화면
    - 새로 시작
    - 이어 하기
    - 업적
    - 제작자
    
  - 세이브 포인트
    
  - 보스
    - 괴물(튜토리얼)
      - 돌진
      - 플레이어에 돌진하는 몬스터 소환
      
      <br/>
    - 2층(소)
      - 내려찍은 후 충격파 발생
      - 돌진
      - 똥 냄새 발생하여 공격 유효하지 않음, 동시에 다른 패턴 가능
      - 사료를 생성하여 보스가 먹으면 hp 회복, 파괴가능
      
    - 3층(호랑이)
      - 포효(기절)
      - 물어뜯기
      - 할퀴기
    
    - 4층(토끼)
      - 공격은 하지 않되, 이동속도가 빠름
      - 시간 제한
    
    - 5층(용)
      - 제자리 불 뿜기
      - 날아다니며 불 뿜기
      
    - 6층(뱀)
      - 물어뜯기
      - 꼬리치기
      - 조이기
      - 이동하는 동안 공격 유효하지 않음

## 배운 점
  - 기획, 아트, 프로그래밍 모두를 혼자 해야하는 것이 처음이었기 때문에 기획서도 생각날 때마다 작성하고, 적합한 리소스를 찾는 시간이 많아 힘들었습니다.
  
  - 기획서를 먼저 작성하고, 기획서를 토대로 구현하는 것이 쉽고 빠르다는 것을 알았습니다.
