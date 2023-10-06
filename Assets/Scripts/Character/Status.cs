using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public float WalkSpeed;
    public float RunSpeed;
    public float MeleeDistance;
    public float TimeBetweenAttacks;
    private float LastAttackTime;
    public float BaseDamage;
    public float MaxLife;
    public float FieldOfVision;
    private float life;

    private void Awake()
    {
        life = MaxLife;
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        if(life<0)
        {
            life = 0;
        }
    }

    public void Heal(float heal)
    {
        life += heal;
        if (life > MaxLife)
        {
            life = MaxLife;
        }
    }

    public float GetLife()
    {
        return life;
    }

    public bool IsAlive()
    {
        return life > 0;
    }

    public bool IsAbleToAttack ()
    {
        if (Time.timeSinceLevelLoad >= LastAttackTime + TimeBetweenAttacks)
        {
            LastAttackTime = Time.timeSinceLevelLoad;
            return true;
        }
        return false;
    }
}
