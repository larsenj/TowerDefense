using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//status effect resulting from being hit by a fire tower
public class FireStatus : StatusEffects
{
    private float ticTime;
    private float timeSinceTic;
    private float ticDamage;

    //ctor
    public FireStatus(float ticDmg, float ticTm, float duration, Mobs tgt) : 
        base(tgt, duration)
    {
        this.ticDamage = ticDmg;
        this.ticTime = ticTm;

       
    }//end ctor

    public override void Update()
    {
        if (target != null)
        {
            //change time and apply damage if enough time has passed
            timeSinceTic += Time.deltaTime;
            if (timeSinceTic >= ticTime)
            {
                timeSinceTic = 0;
                target.TakeDamage(ticDamage, Element.FIRE);
            }
        }

        //call parent class update
        base.Update();
    }//end update

}//end class
