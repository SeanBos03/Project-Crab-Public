using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class PickUpCoin : AnimationSpriteCustom
{
    private int amountOfScore;
    public PickUpCoin(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        amountOfScore = obj.GetIntProperty("f_pickupScoreAmount", 1);
    }

    public int GetScore()
    {
        SoundChannel bulletSound = new Sound("coinPickup.mp3", false, false).Play();
        return amountOfScore;
    }
}