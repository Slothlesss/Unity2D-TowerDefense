using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade
{
    public int Price { get; private set; }
    public int Damage { get; private set; }
    public float AttackCoolDown { get; private set; }
    public float ProcChance { get; private set; }

    public float SlowingFactor { get; private set; }

    public float TickTime { get; private set; }

    public float TickDamage { get; private set; }

    public Sprite sprite { get; private set; }
    public Material material { get; private set; }
    public TowerUpgrade(int price, int damage, 
        float attackCoolDown, float procChance, 
        float slowingFactor, float tickTime, float tickDamage,
        Sprite sprite, Material material)
    {
        this.Price = price;
        this.Damage = damage;
        this.AttackCoolDown = attackCoolDown;
        this.ProcChance = procChance;
        this.SlowingFactor = slowingFactor;
        this.TickTime = tickTime;
        this.TickDamage = tickDamage;
        this.sprite = sprite;
        this.material = material;

    }

}
