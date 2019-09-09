using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public int Level;    
    public int HP;
    public int rewardExp;
    public Status status;

    [Range(0f, 20f)] public float AttackRange;
    [Range(0f, 20f)] public float SearchRange;

    [Range(0f, 3f)] public float MoveSpeed;
    [Range(0f, 3f)] public float AttackSpeed;

    [Range(0, 100)] public int AttackDamage;
    [Range(0f, 100f)] public float CriticalRate;

    public WeaponType WeponType;

    //float DamageReduction;
    //float Evasion;
}

public enum Status { Idle, Chase, Attack }
