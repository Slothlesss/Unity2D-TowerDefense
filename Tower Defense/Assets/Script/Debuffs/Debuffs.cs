using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuffs
{
    protected Monster target;

    protected float duration;
    private float elasped; 
    public Debuffs (Monster target, float duration)
    {
        this.target = target;
        this.duration = duration;
    }
    public virtual void Update()
    {
        elasped += Time.deltaTime;
        if(elasped >= duration)
        {
            Remove();
        }
    }
    public virtual void Remove()
    {
        if(target != null)
        {
            target.RemoveDebuff(this);
        }
    }
}
