using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : BaseItem
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

    private void Start()
    {
        SetDesc($"This weapon deals {damage} damage and adds {attackSpeed * 10} to your attack speed");
    }
}