using System.Collections;
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

    //for adding and subtracting points
    public static Point operator -(Point first, Point second)
    {
        return new Point(first.X - second.X, first.Y - second.Y);
    }
    public static Point operator +(Point first, Point second)
    {
        return new Point(first.X + second.X, first.Y + second.Y);
    }
    
    //for comparing points
    public static bool operator ==(Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }
    public static bool operator !=(Point first, Point second)
    {
        return first.X != second.X || first.Y != second.Y;
    }
}
