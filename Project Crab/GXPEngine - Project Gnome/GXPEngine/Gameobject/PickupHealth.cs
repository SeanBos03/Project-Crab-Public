using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class PickupHealth : AnimationSpriteCustom
{
    int amountOfHealth;
    public PickupHealth(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        amountOfHealth = obj.GetIntProperty("f_pickupHealthAmount", 1);
    }

    public void GetHealth()
    {
        if (GameData.playerHealth < GameData.playerMaxHealth)
        {
            GameData.playerHealth = GameData.playerHealth + amountOfHealth;

            if (GameData.playerHealth > GameData.playerMaxHealth)
            {
                GameData.playerHealth = GameData.playerMaxHealth;
            }
        }
    }
}