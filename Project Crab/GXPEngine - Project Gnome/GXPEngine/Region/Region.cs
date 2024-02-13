using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GXPEngine;
public class Region : Sprite
{
    protected bool actionsAllowed;
    protected int amountOfTimes;

    protected List<TriggerAction> triggerActionList;
    public Region(bool paraActionsAllowed, int scaleX, int scaleY, int amountOfTimes) : base("red1x1.png", false, false)
    {
        triggerActionList = new List<TriggerAction>();
        actionsAllowed = paraActionsAllowed;
        this.scaleX = scaleX;
        this.scaleY = scaleY;
        this.amountOfTimes = amountOfTimes;
        alpha = 0;
    }

    public void AddTriggerAction(TriggerAction triggerAction)
    {
        triggerActionList.Add(triggerAction);
    }

    protected virtual void Update()
    {
        if (actionsAllowed)
        {
            foreach (TriggerAction b in triggerActionList)
            {
                b.Action();
            }
        }

    }
}