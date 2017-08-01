﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//fun fact: C# has structs! The use is they are passed by value into functions,
//whereas classes are passed by reference
public struct Point 
{
    public int X { get; set; }
	
    public int Y { get; set; }

    public Point (int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}