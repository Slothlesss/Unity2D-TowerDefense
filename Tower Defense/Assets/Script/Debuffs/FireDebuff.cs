using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Debuffs
{
    float tickTime;
    float timeSinceTick;
    float tickDamage;
    public FireDebuff(float tickDamage, float tickTime, float duration, Monster target) : base(target, duration)
    {
        this.tickDamage = tickDamage;
        this.tickTime = tickTime;
    }

    // Update is called once per frame
    public override void Update()
    {
        if(target != null)
        {
            timeSinceTick += Time.deltaTime;
            if(timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                target.TakeDamage(tickDamage, Element.Fire);
            }
        }
        base.Update();
    }
}
