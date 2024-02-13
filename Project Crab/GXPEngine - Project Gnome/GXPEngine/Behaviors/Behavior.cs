using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

/*
 * Helps with make NPCs do something
 */
public class Behavior
{
    bool isEnabled; //The task is enabled or not
    protected Character theOwner; //The NPC affected
    protected int theTimer = Time.time;
    protected int actionTimeMin; //The min cooldown time
    protected int actionTimeMax; //The max cooldown time
    protected int actionPerformTime; //The time doing the action
    public Behavior(Character theOwner, int actionTimeMin, int actionTimeMax, int paraPerformTime)
    {
        isEnabled = true;
        this.theOwner = theOwner;
        this.actionTimeMin = actionTimeMin;
        this.actionTimeMax = actionTimeMax;
        actionPerformTime = paraPerformTime;
    }
    public virtual void Action()
    {
        if (!isEnabled)
        {
            return;
        }
    }

    public void ChangeOwner(Character theOwner)
    {
        this.theOwner = theOwner;
    }

}