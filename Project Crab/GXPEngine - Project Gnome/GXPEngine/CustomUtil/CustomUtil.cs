using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using GXPEngine.Core;
using GXPEngine;

/*
 * Providing functions that all classes can use
 */
public static class CustomUtil
{
    static CustomUtil()
    {
    }

    //Check if a point is within a rectangle shape
    public static bool CheckPointWithRect(Vector2 boxCrood, int boxWidth, int boxHeight, Vector2 otherCrood)
    {
        if (boxCrood.x - boxWidth / 2 <= otherCrood.x && boxCrood.x + boxWidth / 2 >= otherCrood.x)
        {
            if (boxCrood.y - boxHeight / 2 <= otherCrood.y && boxCrood.y + boxHeight / 2 >= otherCrood.y)
            {
                return true;
            }
        }
        return false;
    }

    //Check if two AnimationSpriteCustom objects intersect
    public static bool IntersectsAnimationSpriteCustom(AnimationSpriteCustom thisObject, AnimationSpriteCustom thatObject)
    {
        if (Math.Abs(thisObject.x - thatObject.x) <= (thisObject.width / 2) + (thatObject.width / 2) &&
            Math.Abs(thisObject.y - thatObject.y) <= (thisObject.height / 2) + (thatObject.width / 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //same as the method above but with different parameter type (a sprite and  AnimationSprtieCustom)
    public static bool IntersectsSpriteCustomAndAnimationSpriteCustom(Sprite thisObject, AnimationSpriteCustom thatObject)
    {
        if (Math.Abs(thisObject.x - thatObject.x) <= (thisObject.width / 2) + (thatObject.width / 2) && 
            Math.Abs(thisObject.y - thatObject.y) <= (thisObject.height / 2) + (thatObject.width / 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}