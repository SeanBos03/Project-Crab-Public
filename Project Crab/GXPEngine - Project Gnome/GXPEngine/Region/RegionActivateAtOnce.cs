using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
public class RegionActivateAtOnce : Region
{
    bool inRegion;
    AnimationSpriteCustom theActivator;
    int theTimer = Time.time;
    public RegionActivateAtOnce(AnimationSpriteCustom paraTheActivator, bool paraActionsAllowed, int scaleX, int scaleY, int amountOfTimes) : 
        base(paraActionsAllowed, scaleX, scaleY, amountOfTimes)
    {
        theActivator = paraTheActivator;
        actionsAllowed = paraActionsAllowed;
        inRegion = false;
    }

    public void ChangeActivator(AnimationSpriteCustom paraTheActivator)
    {
        theActivator = paraTheActivator;
    }

    protected override void Update()
    {
        if (Time.time - theTimer >= 100)
        {
            CheckIntersect(theActivator);
            base.Update();
            theTimer = Time.time;
        }
    }

    void CheckIntersect(AnimationSpriteCustom other)
    {
        if (amountOfTimes == 0)
        {
            actionsAllowed = false;
            return;
        }
        /*
        Console.WriteLine("P: {0}, {1} | {2}x{3} | R: {4}x{5} | {6}x{7}", theActivator.x, theActivator.y, theActivator.width, theActivator.height,
            x, y, width, height);
        */
        bool isIntersect = CustomUtil.IntersectsSpriteCustomAndAnimationSpriteCustom(this, theActivator);

        if (isIntersect == false && inRegion == true)
        {
            if (amountOfTimes != -1)
            {
                amountOfTimes--;
            }

            inRegion = false;
            actionsAllowed = true;
            return;
        }

        if (isIntersect == true && inRegion == false)
        {
            inRegion = true;
            actionsAllowed = true;
            return;
        }

        if (isIntersect == true && inRegion == true)
        {
            actionsAllowed = false;
            return;
        }

        if (isIntersect == false && inRegion == false)
        {
            actionsAllowed = false;
            return;
        }

    }

}