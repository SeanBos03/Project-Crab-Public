using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TriggerActionVisbilityTimed : TriggerAction
{
    bool isOn;
    int theTimer = Time.time;
    int theTime;
    public TriggerActionVisbilityTimed(GameObject theOwner, bool isOn, int theTime)
        : base(theOwner, 0, 0, 0, "TriggerActionVisbility")
    {
        Console.WriteLine("displaying");
        this.isOn = isOn;
        this.theTime = theTime;
        triggerActionID = "TriggerActionVisbilityTimed";
        toggleOBjectEnable();
    }

    public override void Action()
    {
        base.Action();

        if (Time.time - theTimer >= theTime)
        {
            toggleOBjectEnable();
            finished = true;
            Console.WriteLine("done displaying");
        }
    }

    void toggleOBjectEnable()
    {
        if (isOn)
        {
            theOwner.visible = false;
            isOn = false;
        }

        else
        {
            theOwner.visible = true;
            isOn = true;
        }
    }
}
