using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    [SerializeField]
    private AudioClip attackSfx = null;
    [SerializeField]
    private GameObject meleeEffect = null;

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
                this.animator.SetBool("IsChasing", false);
                this.animator.SetBool("IsAttacking", true);
                //this.agent.isStopped = true;

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
        // 플레이어와의 거리를 기준으로 상태를 결정한다.
        float dist = Vector3.Distance(target.transform.position, this.transform.position);

        if (dist < enemyInfo.AttackRange)
        {
            // 사정거리 내에 있을 때 공격 시작.
            this.enemyInfo.status = Status.Attack;
        }
        else if (dist < enemyInfo.SearchRange)
        {
            // 시야 내에 있을 때 추적 시작.
            this.enemyInfo.status = Status.Chase;
        }
        else
        {
            this.enemyInfo.status = Status.Idle;
        }
    }

    protected override void AttackTarget()
    {
        if (this.target == null)
            return;

        // PumpFramework.Common.SoundManager.Instance.PlaySound(attackSfx, this.transform.position);

        // 근접 공격
        GameObject attack = Instantiate(meleeEffect, this.transform);
        attack.transform.Translate(Vector3.forward * 2, Space.Self);
        attack.transform.localScale = new Vector3(2, 1, 3);
        
        if (Random.Range(0, 100f) < this.enemyInfo.CriticalRate)
            attack.GetComponent<MeleeEffect>().Damage = this.enemyInfo.AttackDamage * 2;
        else
            attack.GetComponent<MeleeEffect>().Damage = this.enemyInfo.AttackDamage;
            
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