using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class PickupJumpSpeedUpgrade : AnimationSpriteCustom
{
    public int theIndex;
    public float theAmount;
    public string theTextDisplay;

    public PickupJumpSpeedUpgrade(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        theIndex = obj.GetIntProperty("f_theIndex", -1);
        theAmount = obj.GetFloatProperty("f_theAmount", 0);
        theTextDisplay = obj.GetStringProperty("f_theTextCanvasDisplay", "");
    }
}