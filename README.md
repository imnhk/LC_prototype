# LC_prototype

 C# scripts used in prototype of mobile shooter, "Legendary Commando". Result of 2019 summer internship at Softpump.


# Screenshots

![lc1](https://user-images.githubusercontent.com/16031834/65022873-b3568d00-d96c-11e9-9275-a3196c4a63ff.png)
![lc2](https://user-images.githubusercontent.com/16031834/65022883-b5b8e700-d96c-11e9-934e-95bbe601725c.png)


# Included script

## /Enemy

EnemyBase.cs
- 적 캐릭터 클래스가 상속받는 Base 클래스.

EnemyInfo.cs
- 적 캐릭터들의 상태와 수치를 저장하는 클래스.

BasicMaskMan.cs
, Gaenaider.cs
, MeleeEnemy.cs
- 세 가지 타입의 적 클래스.
    
EnemyBullet.cs
Explosion.cs
MeleeEffect.cs
Grenade.cs
- 각각 적의 공격 방식을 구현하는 클래스.


## /Gameplay

ItemGenerator.cs
NPCGenerator.cs
- 맵 에디터로 생성한 Scene 정보에 따라 아이템과 적을 생성하는 스크립트.

MapGenerator.cs
- 선택된 맵에 따라 Scene을 불러오는 스크립트.

PlayerControl.cs
- 맵에 플레이어 캐릭터를 생성한 후 UI를 통해 조작 입력을 받는 스크립트.

## ~~/Item~~
(빌드에 포함되지 않음)

## /Manager

CameraManager.cs
- 플레이어이 위치에 따라 카메라를 움직이는 스크립트.

LobbyManager.cs
- 데모 버전에서 플레이할 맵을 선택하는 Scene에 사용.

ObjectPooler.cs
- 최적화를 위해 Object Pooling을 구현한 클래스. PlayerBullet, EnemyBullet 클래스에 적용되었다.

PlayUIManager.cs
DamagePopup.cs
- 체력, 피해량, 버튼, 조이스틱 등 UI를 화면에 표시하는 클래스.

## /MapEditor

EnemySpawnSpot.cs
ItemSpawnSpot.cs
PlayerStartSpot.cs
- 플레이어와 적, 아이템이 생성될 위치를 맵 에디터에서 표현하는 데 사용.

MapEditor.unity
MapEditor.cs
MapMakerEditor.cs
- 사용될 맵의 Scene을 만들기 위한 에디터 구현.

## /Player

ShootingStateAnimation.cs
- 플레이어의 현재 공격 방식에 따라 애니메이션을 변경하는 StateMachineBehavior 클래스.

