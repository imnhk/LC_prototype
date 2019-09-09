using UnityEngine;

public class BasicMaskMan : EnemyBase
{
    [SerializeField]
    private GameObject bulletPrefab = null;
    [SerializeField]
    private AudioClip attackSfx = null;

    private float statusCheckTime = 0;
    private float statusCheckInterval = 0.5f;

    protected override void FixedUpdate()
    {
        if (!this.IsAlive)
        {
            return;
        }

        if (!this.HasTarget)
        {
            this.animator.SetBool("IsAttacking", false);
            this.animator.SetBool("IsChasing", false);
            this.agent.isStopped = true;
            return;
        }

        this.SetCurrentStatus();

        switch (this.enemyInfo.status)
        {
            case Status.Idle:
                this.animator.SetBool("IsAttacking", false);
                this.animator.SetBool("IsChasing", false);
                this.agent.isStopped = true;

                break;

            case Status.Chase:
                this.animator.SetBool("IsAttacking", false);
                this.animator.SetBool("IsChasing", true);
                this.agent.isStopped = false;

                this.agent.SetDestination(this.target.transform.position);
                this.agent.speed = this.enemyInfo.MoveSpeed;
                this.animator.speed = this.enemyInfo.MoveSpeed;
                break;

            case Status.Attack:
                this.animator.SetBool("IsAttacking", true);
                this.animator.SetBool("IsChasing", false);
                this.agent.isStopped = true;

                this.animator.speed = this.enemyInfo.AttackSpeed;

                Vector3 targetDir = this.target.transform.position - this.transform.position;
                this.transform.rotation = Quaternion.LookRotation(targetDir);
                break;

            default:
                break;
        }        
    }

    protected override void SetCurrentStatus()
    {
        // 매 프레임 확인할 필요는 없다.
        if(Time.time < this.statusCheckTime + this.statusCheckInterval)
            return;

        float dist = Vector3.Distance(this.transform.position, target.transform.position);

        // 적 탐지 거리
        if(dist > enemyInfo.SearchRange)
            return;

        this.statusCheckTime = Time.time;

        Vector3 firingPos = this.transform.position + new Vector3(0, 2, 0);
        Vector3 targetPos = this.target.transform.position + new Vector3(0, 2, 0);
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Obstacle", "Player");

        if (Physics.Raycast(firingPos, (targetPos - firingPos), out hit, Mathf.Infinity, layerMask))
        {
            // Player는 Collider가 아니라 CharacterController을 사용하므로 이렇게 해야 한다.
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

    protected override void AttackTarget()
    {
        if (this.target == null)
        {
            return;
        }            

        Vector3 firingPos = this.transform.position + new Vector3(0, 2, 0);
        Vector3 targetPos = this.target.transform.position + new Vector3(0, 2, 0);

        // 발사 후 총알 오브젝트의 위력을 설정한다.
        //GameObject bullet = Instantiate(bulletPrefab, firingPos, Quaternion.LookRotation(targetPos - firingPos, Vector3.up));
        GameObject bullet = ObjectPooler.Instance.enemyBullet.GetObject;
        bullet.transform.position = firingPos;
        bullet.transform.rotation = Quaternion.LookRotation(targetPos - firingPos, Vector3.up);
        bullet.SetActive(true);

        // 너무 시끄럽다.
        // PumpFramework.Common.SoundManager.Instance.PlaySound(attackSfx, this.transform.position);


        if (Random.Range(0, 100f) < this.enemyInfo.CriticalRate)
            bullet.GetComponent<EnemyBullet>().Damage = this.enemyInfo.AttackDamage * 2;
        else
            bullet.GetComponent<EnemyBullet>().Damage = this.enemyInfo.AttackDamage;
    }

    protected override void Die()
    {
        this.IsAlive = false;
        this.animator.SetTrigger("Die");
        this.GetComponent<BoxCollider>().enabled = false;
        NPCGenerator.Instance.Enemies.Remove(this);
        this.agent.isStopped = true;

        // 플레이어 경험치 증가
        PlayerControl.Instance.Player.GetComponent<Player>().info.Exp += this.enemyInfo.rewardExp;        
    }
}