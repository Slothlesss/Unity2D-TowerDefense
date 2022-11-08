using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireTower : Tower
{
    [SerializeField]
    float tickTime;
    [SerializeField]
    float tickDamage;
    [SerializeField] List<Sprite> spriteList;
    [SerializeField] List<Material> materialList;

    public float TickTime
    { 
        get 
        {
            return tickTime;
        } 
    }
    public float TickDamage
    { 
        get 
        { 
            return tickDamage; 
        }
    }

    private void Start()
    {
        ElementType = Element.Fire;
        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(10,5,-0.2f,10,0, -0.1f,5, spriteList[0], materialList[0]),
            new TowerUpgrade(20,10, -0.5f,20,0, -0.3f,10,spriteList[1], materialList[1]),
            new TowerUpgrade(30,20, -1f,30,0, -1f,15,spriteList[2], materialList[2])
        };
    }
    public override Debuffs GetDebuff()
    {
        return new FireDebuff(tickDamage, tickTime, DebuffDuration,Target);
    }

    public override void Upgrade()
    {
        this.tickTime -= NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.TickDamage;
        base.Upgrade();

    }

}
