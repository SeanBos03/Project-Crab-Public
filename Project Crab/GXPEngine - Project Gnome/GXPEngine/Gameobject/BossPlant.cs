using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

/*
 * Plant that player must collect to win the boss fight
 */
public class BossPlant : AnimationSpriteCustom
{
    public int amountOfScore; //how much boss health decreases
    public BossPlant(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        amountOfScore = obj.GetIntProperty("f_pickupScoreAmount", 1);
    }
}