using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TriggerActionVisbility : TriggerAction
{
    bool isOn;
    public TriggerActionVisbility(GameObject theOwner, bool isOn)
        : base(theOwner, 0, 0, 0, "TriggerActionVisbility")
    {
        this.isOn = isOn;
    }

    public override void Action()
    {
        base.Action();
        toggleOBjectEnable();
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
