using UnityEngine;
using UnityEngine.AI;

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

    protected abstract void FixedUpdate();
    protected abstract void SetCurrentStatus();
    protected abstract void AttackTarget();
    protected abstract void Die();
}
