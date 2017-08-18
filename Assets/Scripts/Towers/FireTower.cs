using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Towers
{
    //how often the effect applies
    [SerializeField]
    private float ticTime;
    public float TicTime { get { return ticTime; } }

    //how much damage per tic
    [SerializeField]
    private float ticDamage;
    public float TicDamage { get { return ticDamage;  } }

    private void Start()
    {
        elementType = Element.FIRE;
    }

    public override StatusEffects GetEffect()
    {
        return new FireStatus(TicDamage, TicTime, EffectDuration, TargetMob);
        //throw new NotImplementedException();
    }

}
