using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturePower : Tower
{
    private void Start()
    {
        ElementType = Element.Nature;
    }
    public override Debuffs GetDebuff()
    {
        return new NatureDebuff(Target); 
    }
}
