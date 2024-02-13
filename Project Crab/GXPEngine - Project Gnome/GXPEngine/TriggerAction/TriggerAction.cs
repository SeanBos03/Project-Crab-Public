using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class TriggerAction
{
    public bool finished = false;
    public string triggerActionID;
    protected bool isEnabled;
    protected GameObject theOwner;
    protected int actionTimeMin;
    protected int actionTimeMax;
    protected int performTime;
    public TriggerAction(GameObject theOwner, int actionTimeMin, int actionTimeMax, int performTime, string triggerActionID)
    {
        isEnabled = true;
        this.triggerActionID = triggerActionID;
        this.theOwner = theOwner;
        this.actionTimeMin = actionTimeMin;
        this.actionTimeMax = actionTimeMax;
        this.performTime = performTime;
    }
    public virtual void Action()
    {
        if (!isEnabled)
        {
            return;
        }
    }

    public void SetGameObject(Character paraTheGameObject)
    {
        theOwner = paraTheGameObject;
    }

    public string BehaviorID
    {
        get { return triggerActionID; }
        private set { }
    }
}