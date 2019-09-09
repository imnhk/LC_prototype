using UnityEngine;

public enum WeaponType { Grenade = 0, Once = 1, Burst = 2, Loop = 3, Melee = 4 }


[System.Serializable]
public struct PlayerInfo
{
    public int Level;
    public int Exp;

    public WeaponType weaponType;

    [Range(0f, 2f)]
    public float MoveSpeed;

    [Range(0, 1000)]
    public int HP;
    [Range(0, 1000)]
    public int HPMax;
    public int HPRecovery;
    public int Shield;
    public float ShieldRecovery;

    [Range(0, 100)]
    public int AttackDamage;
    //public float AttackRange;
    [Range(0f, 2f)]
    public float AttackSpeed;

    [Range(5, 100)]
    public float CriticalRate;    

    //public float DamageReduction;
    //public float Dodge;
}
