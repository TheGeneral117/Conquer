using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMonoWeapon : NonMonoBaseItem
{
    [SerializeField] int damage = 0;
    [SerializeField] float attackSpeed = 0;

    public int GetAttackDamage()
    {
        return damage;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public void SetAttackDamage(int _damage)
    {
        damage = _damage;
    }

    public void SetAttackSpeed(float _attackspeed)
    {
        attackSpeed = _attackspeed;
    }
}