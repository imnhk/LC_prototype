using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenaider : EnemyBase
{
    [SerializeField] GameObject granadeObject;

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

        // 수류탄 공격
        Grenade grenade = Instantiate(granadeObject, this.transform.position, this.transform.rotation).GetComponent<Grenade>();
        grenade.targetPos = target.transform.position;

        if (Random.Range(0, 100f) < this.enemyInfo.CriticalRate)
            grenade.damage = this.enemyInfo.AttackDamage * 2;
        else
            grenade.damage = this.enemyInfo.AttackDamage;
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
