using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthStatus : StatusEffects
{
    private float slowFactor;

    private bool slowed;

    //ctor
    public EarthStatus(float slowFactor, float duration, Mobs tgt) : 
        base(tgt, duration)
    {
        this.slowFactor = slowFactor;
    }//end ctor

    public override void Update()
    {
        if(target != null)
        {
            //if target not already slowed, apply the debuff
            if(!slowed)
            {
                slowed = true;
                target.Speed -= (target.MaxSpeed * slowFactor) / 100;
            }

        }
        base.Update();
    }//end Update

    //remove the debuff
    public override void Remove()
    {
        //return to normal speed
        if(target != null)
            target.Speed = target.MaxSpeed;
        base.Remove();
    }

}//end class
