﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTower : Towers
{
    private void Start()
    {
        elementType = Element.WIND;
    }

    public override StatusEffects GetEffect()
    {
        return new WindStatus(TargetMob);
        //throw new NotImplementedException();
    }

}
