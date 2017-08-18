using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthTower : Towers
{
    //how much the tower slows mobs by
    [SerializeField]
    private float slowFactor;
    //public float SlowFactor { get { return slowFactor; } }

    private void Start()
    {
        elementType = Element.EARTH;
    }

    public override StatusEffects GetEffect()
    {
        return new EarthStatus(slowFactor, EffectDuration, TargetMob);
        //throw new NotImplementedException();
    }

}
