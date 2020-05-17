# LC_prototype

 C# scripts used in prototype of mobile shooter, "Legendary Commando". Result of 2019 summer internship at Softpump.

## Video
[![LC](https://user-images.githubusercontent.com/16031834/65022873-b3568d00-d96c-11e9-9275-a3196c4a63ff.png)](https://youtu.be/_meiPh_OApg "Video link")

## Screenshots

![lc1](https://user-images.githubusercontent.com/16031834/65022883-b5b8e700-d96c-11e9-934e-95bbe601725c.png)


## Included script

### /Enemy

EnemyBase.cs
- 적 캐릭터 클래스가 상속받는 abstract 클래스.
~~~
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]
    protected EnemyInfo enemyInfo;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected Player target = null;

    public bool IsAlive { get; set; }
    public int CurrentHp { get { return this.enemyInfo.HP; } }
    public bool HasTarget { get { return (this.target != null) && (this.target.IsAlive); } }

    protected void Awake()
    {
        this.agent = this.GetComponent<NavMeshAgent>();
        this.animator = this.GetComponent<Animator>();
    }

    protected void Start()
    {
        this.IsAlive = true;
        this.target = PlayerControl.Instance.Player.GetComponent<Player>();
        this.enemyInfo.status = Status.Idle;
    }

    // 피해를 받을 때의 효과는 Enemy 모두 동일
    public void TakeDamage(int damage)
    {
        if (!this.IsAlive)
        {
            return;
        }            

        this.enemyInfo.HP -= damage;
        this.animator.SetTrigger("TakeDamage");

        PlayUIManager.Instance.CreateDamagePopup(damage, this.gameObject.transform.position);

        if (this.enemyInfo.HP <= 0)
        {
            this.enemyInfo.HP = 0;
            this.Die();
        }
    }

    // 이외 Status, 공격, 사망 시 행동은 하위 클래스에서 구현
    protected abstract void FixedUpdate();
    protected abstract void SetCurrentStatus();
    protected abstract void AttackTarget();
    protected abstract void Die();
}
~~~

EnemyInfo.cs
- 적 캐릭터들의 상태와 수치를 저장하는 클래스.

BasicMaskMan.cs
, Grenaider.cs
, MeleeEnemy.cs
- 세 가지 타입의 적 클래스.

플레이어와의 거리, 장애물의 유무를 확인해 Status를 결정하는 함수
~~~
protected override void SetCurrentStatus()
{
        // 매 프레임 확인할 필요는 없음
        if(Time.time < this.statusCheckTime + this.statusCheckInterval)
            return;
        float dist = Vector3.Distance(this.transform.position, target.transform.position);

        if(dist > enemyInfo.SearchRange)
            return;

        this.statusCheckTime = Time.time;

        Vector3 firingPos = this.transform.position + new Vector3(0, 2, 0);
        Vector3 targetPos = this.target.transform.position + new Vector3(0, 2, 0);
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Obstacle", "Player");

        // Raycast를 사용해 장애물 확인
        if (Physics.Raycast(firingPos, (targetPos - firingPos), out hit, Mathf.Infinity, layerMask))
        {
            // Player는 Collider가 아니라 CharacterController을 사용하므로 tag를 사용해야 한다
            if (hit.collider.transform.tag == "Player")
            {
                // 장애물 없음. 공격 상태로 전환.
                this.enemyInfo.status = Status.Attack;
            }
            else
            {
                // 장애물 있음. NavMesh 따라 이동.
                this.enemyInfo.status = Status.Chase;
            }
        }
}
~~~
    
    
EnemyBullet.cs
Explosion.cs
MeleeEffect.cs
Grenade.cs
- 각각 적의 세부 공격 방식을 구현하는 클래스.


### /Gameplay

Player.cs
- 플레이어 이동, 공격, 이펙트, 애니메이션 등 메인 게임플레이 처리.

PlayerControl.cs
- 맵에 플레이어 캐릭터를 생성한 후 공격방식 변경 입력을 처리.

PlayerBullet.cs
- 플레이어 공격 탄환의 이동과 충돌 확인을 처리.

PlayerInfo.cs
- 플레이어 캐릭터에 대한 정보를 저장하는 Serializable 클래스.

### ~~/Item~~
(빌드에 포함되지 않음)

인벤토리, 아이템 UI panel 클릭/드래그, 아이템 레벨업 등
~~~
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;     // UI이벤트 사용을 위해

public class DragablePanel : MonoBehaviour,
    IPointerDownHandler,        // 마우스 클릭
    IDragHandler,               // 마우스 드래그 
    IPointerUpHandler           // 마우스 클릭 종료 인터페이스
{

    private Canvas parentCanvas = null;         
    private bool isDragging = false;            
    private Vector2 eventPos = Vector2.zero;    
    private Vector2 startOffset = Vector2.zero; //드래그 시작위치 오프셋


    void Awake()
    {
        this.parentCanvas = this.GetComponentInParent<Canvas>();
    }
	
	void Update ()
    {
        if (this.isDragging)
        {
            // 이벤트 위치를 Canvas Local 위치로 변경
            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.parentCanvas.transform as RectTransform,
                eventPos,
                this.parentCanvas.worldCamera,
                out localPos);

            // 최종 localPosition 변경
            Vector2 finalLocalPos = localPos;
            this.transform.localPosition = startOffset + finalLocalPos;
        }	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        // 제일 위에 그려져야 한다
        this.transform.SetAsLastSibling();

        isDragging = true;             
        // 이벤트 위치 설정
        eventPos = eventData.position; 

        // eventPos를 local 위치로 변환
        Vector2 localOffset = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.parentCanvas.transform as RectTransform,
            eventPos,
            this.parentCanvas.worldCamera,
            out localOffset);

        this.startOffset.x = this.transform.localPosition.x - localOffset.x;
        this.startOffset.y = this.transform.localPosition.y - localOffset.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중 event 위치 갱신
        eventPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}

~~~

### /Manager

CameraManager.cs
- 플레이어이 위치에 따라 카메라를 움직이는 스크립트.

LobbyManager.cs
- 데모 버전에서 플레이할 맵을 선택하는 Scene에 사용.

ObjectPooler.cs
- 최적화를 위해 Object Pooling을 구현한 클래스. PlayerBullet, EnemyBullet 클래스에 적용되었다.

~~~
[System.Serializable]
public class ObjectToPool
{
    // pool 내 비활성화된 Gameobject 하나를 반환하는 property
    public GameObject GetObject
    {
        get
        {
            for (int i = 0; i < amount; i++)
            {
                if (!pool[i].activeInHierarchy)
                    return pool[i];
            }

            Debug.LogError("Object pool is full");
            return null;
        }
    }

    public int amount;
    public GameObject gameObject;
    [System.NonSerialized]
    public List<GameObject> pool;
}
~~~
초기화 함수
~~~
private void Initialize(ObjectToPool poolObject)
    {
        poolObject.pool = new List<GameObject>();
        for (int i = 0; i < poolObject.amount; i++)
        {
            GameObject newObj = Instantiate(poolObject.gameObject, objectPool);
            newObj.SetActive(false);
            poolObject.pool.Add(newObj);
        }
    }
~~~

PlayUIManager.cs
DamagePopup.cs
- 체력, 피해량, 버튼, 조이스틱 등 UI를 화면에 표시하는 클래스.

### /MapEditor

EnemySpawnSpot.cs
ItemSpawnSpot.cs
PlayerStartSpot.cs
- 플레이어와 적, 아이템이 생성될 위치를 맵 에디터 Gizmo로 표현하는 데 사용.

MapGenerator.cs
- 선택된 맵에 따라 Scene을 불러오는 스크립트.

MapEditor.unity
MapMaker.cs

~~~
public void CreateMap()
    {
        GameObject go = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity);
        go.name = "New Map";

        ItemSpawnSpot[] itemSpots = go.transform.GetComponentsInChildren<ItemSpawnSpot>();
        EnemySpawnSpot[] enemySpots = go.transform.GetComponentsInChildren<EnemySpawnSpot>();

        for (var i = 0; i < itemSpots.Length; ++i)
        {
            Destroy(itemSpots[i]);
        }

        for (var i = 0; i < enemySpots.Length; ++i)
        {
            Destroy(enemySpots[i]);
        }

        Destroy(go.GetComponent<MapMaker>());
    }
~~~

ItemGenerator.cs
NPCGenerator.cs
- 맵 에디터로 생성한 Scene 정보에 따라 아이템과 적을 생성하는 스크립트.
~~~
private void Start()
    {
        Transform[] itemSpots = this.itemSpawnSpots.GetComponentsInChildren<Transform>();
        foreach (Transform spot in itemSpots)
        {
            // 부모 오브젝트는 제외한다
            if (spot.name == this.itemSpawnSpots.name)
                continue;

            GameObject item = Instantiate<GameObject>(medicBoxPrefab, spot.position, spot.rotation);
            item.transform.parent = this.transform;
        }
    }
~~~
MapMakerEditor.cs
- 사용될 맵의 Scene을 만들기 위한 inspector GUI 구현.
~~~
public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();

        MapMaker myScript = (MapMaker)this.target;

        if (GUILayout.Button("Add Item"))
        {
            myScript.AddItemSpot();
        }

        if (GUILayout.Button("Add Enemy"))
        {
            myScript.AddEnemySpot();
        }

        if (GUILayout.Button("Create Map"))
        {
            myScript.CreateMap();
        }
    }
~~~
