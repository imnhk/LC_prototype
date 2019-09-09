using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public PlayerInfo info;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] List<GameObject> weaponObjects = new List<GameObject>();
    [SerializeField] AudioClip gunFireAudio;
    [SerializeField] AudioClip hitAudio;
    [SerializeField] AudioClip getItemAudio;

    public GameObject Target { get { return target.gameObject; } }
    public bool HasTarget { get { return target != null; } }
    private EnemyBase target = null;

    private CharacterController characterController;

    public Animator Animator { get { return animator; } }
    private Animator animator;

    private Transform weaponSlotTransform;
    private ParticleSystem gunPointParticle;

    private float hpRegenCounter = 0;

    public bool IsMoving { get { return this.characterController.velocity.magnitude > 0; } set { } }
    public bool IsAlive { get; set; }
    public float HpPercent { get { return (float)info.HP / info.HPMax; } }
    // 무기 타입에 따른 애니메이션 변경을 위해 쓰임

    void Awake()
    {
        // 무기를 생성하고 손에 위치시킨다.
        weaponSlotTransform = GameObject.Find("WeaponSlot").GetComponent<Transform>();
        GameObject weapon = Instantiate<GameObject>(weaponObjects[(int)info.weaponType], this.weaponSlotTransform, false);

        gunPointParticle = GameObject.Find("Gunpoint").GetComponent<ParticleSystem>();

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        IsAlive = true;

        if (PlayerPrefs.HasKey("PlayerInfo"))
        {
            string playerInfoJson = PlayerPrefs.GetString("PlayerInfo");
            info = JsonUtility.FromJson<PlayerInfo>(playerInfoJson);
        }
        if (info.HP <= 0)
        {
            Die();
        }
        info.HP = Mathf.Min(info.HPMax, info.HP);

    }

    void FixedUpdate()
    {
        if (!IsAlive)
        {
            // Debug.Log("Game Over");
            return;
        }

        // 가장 가까운 적을 Target으로 설정
        target = NPCGenerator.Instance.GetClosestEnemyFrom(this.transform.position);


        hpRegenCounter += Time.fixedDeltaTime;
        if (hpRegenCounter > .5f)
        {
            info.HP += info.HPRecovery;
            info.HP = Mathf.Min(info.HPMax, info.HP);
            hpRegenCounter = 0;
        }


        // PlayerControl에서 키보드 입력을 받는다.
        if(PlayerControl.Instance.MovementInput.x != 0 || PlayerControl.Instance.MovementInput.z != 0)
        {
            // 플레이어 이동
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsMoving", true);

            characterController.Move(PlayerControl.Instance.MovementInput * Time.deltaTime * info.MoveSpeed);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        // 플레이어가 이동하지 않음(속도 <= 0)
        if (!IsMoving)
        {
            // Target이 있을 시 공격 애니메이션 재생
            animator.SetBool("IsAttacking", HasTarget);
        }

        // 플레이어 캐릭터가 바라보는 방향 설정
        if (animator.GetBool("IsAttacking"))
        {
            // 공격 도중 Target을 바라본다.
            Vector3 targetDir = target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(targetDir);
            
        }
        else
        {
            if(PlayerControl.Instance.MovementInput.magnitude > 0)
                transform.rotation = Quaternion.LookRotation(PlayerControl.Instance.MovementInput);
        }
    }

    public void AttackTarget()
    {
        if (target == null)
            return;

        Vector3 firingPos = this.transform.position + new Vector3(0, 2, 0);
        Vector3 targetPos = target.transform.position + new Vector3(0, 2, 0);

        // 발사 방향에 맞게 총기 모델을 회전시킨다
        this.weaponSlotTransform.transform.rotation = Quaternion.LookRotation(targetPos - firingPos);

        // 총구 이펙트 재생
        this.gunPointParticle.Play();

        // 발사 효과음 재생
        PumpFramework.Common.SoundManager.Instance.PlaySound(gunFireAudio, gunPointParticle.transform.position, false);


        // 발사 후 총알 오브젝트의 위력을 설정한다.
        //GameObject bullet = Instantiate(bulletPrefab, firingPos, Quaternion.LookRotation(targetPos - firingPos, Vector3.up));
        GameObject bullet = ObjectPooler.Instance.playerBullet.GetObject;
        bullet.transform.position = firingPos;
        bullet.transform.rotation = Quaternion.LookRotation(targetPos - firingPos, Vector3.up);
        bullet.SetActive(true);

        if (Random.Range(0, 100f) < info.CriticalRate )
        {
            bullet.GetComponent<PlayerBullet>().Damage = info.AttackDamage * 2;
        }
        else
        {
            bullet.GetComponent<PlayerBullet>().Damage = info.AttackDamage;
        }

        #region OldRaycastCode

        /*
        int layerMask = LayerMask.GetMask("Obstacle", "Enemy");
        Vector3 firingPos = this.transform.position + new Vector3(0, 2, 0);

        // Raycast로 공격 시 장애물 확인
        RaycastHit hit;
        if (Physics.Raycast(firingPos, (target.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 2, Random.Range(-0.5f, 0.5f)) - firingPos), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawLine(firingPos, hit.point, Color.yellow, 0.3f);

            // Weapon.hitEffect.transform.position = hit.point;
            Weapon.muzzleFlash.GetComponent<ParticleSystem>().Play();
            
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(atk);
            }
        }*/

        #endregion
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive)
            return;

        info.HP -= damage;

        // 피해량 팝업 생성
        PlayUIManager.Instance.CreateDamagePopup(damage, this.gameObject.transform.position);

        // 피격 효과음 재생
        PumpFramework.Common.SoundManager.Instance.PlaySound(hitAudio, this.transform.position, false);

        if (info.HP <= 0)
        {
            info.HP = 0;
            Die();
        }
    }

    private void Die()
    {
        info.HP = 0;
        IsAlive = false;
        animator.SetTrigger("Die");
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템 획특 처리
        if (other.tag == "Item")
        {
            switch (other.name)
            {
                case "MedicBox(Clone)":
                    info.HP += 50;
                    info.HP = Mathf.Min(info.HPMax, info.HP);
                    PumpFramework.Common.SoundManager.Instance.PlaySound(getItemAudio, this.transform.position, false);
                    //ItemDatabase.Instance.Add(new Item(ItemType.Armor, 1, 123, ItemDatabase.Instance.icons[36]));
                    break;

                default:
                    break;
            }
            
            Destroy(other.gameObject);                      
        }
    }
}