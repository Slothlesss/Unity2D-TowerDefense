using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPower : Tower
{
    // Start is called before the first frame update
    private void Start()
    {
        ElementType = Element.Electric;
    }
    public override Debuffs GetDebuff()
    {
        return new ElectricDebuff(Target);
    }
}
