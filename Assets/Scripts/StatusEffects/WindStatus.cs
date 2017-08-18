using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wind has no status effect, the tower simply fires faster
public class WindStatus : StatusEffects
{
    //ctor
    public WindStatus(Mobs tgt) : base(tgt, 0)
    {

    }//end ctor

    public override void Update()
    {
        base.Update();
    }
    
}
