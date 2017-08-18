using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for the various debuffs/status effects
public abstract class StatusEffects
{
    protected Mobs target;

    private float duration;

    protected float timeElapsed;

    public StatusEffects(Mobs tgt, float duration)
    {
        this.target = tgt;
        this.duration = duration;
    }

    //virtual so can be overridden in child classes
    public virtual void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= duration)
            Remove();
    }

    public virtual void Remove()
    {
        if(target != null)
        {
            target.RemoveEffect(this);
        }

            

    }//end remove

}//end class StatusEffects
