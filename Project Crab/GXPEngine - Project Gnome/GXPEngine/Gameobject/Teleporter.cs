using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

/*
 * 
 * Helps for providing data to make a gameobject move
 * to a specific crood or another gameobject's crood
 * 
 */
public class Teleporter : AnimationSpriteCustom
{
    public AnimationSpriteCustom theOwner;  //the gameobject that will be teleported
    public AnimationSpriteCustom theTarget;  //theOwner will get teleported to theTarget's crood, if teleportToObject = true
    public bool teleportToObject;
    public int teleportCroodX; //if teleportToObject = false, theOwner would be teleport to the specified crood
    public int teleportCroodY;
    public Teleporter(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        teleportToObject = obj.GetBoolProperty("f_teleportToObject", false);
        teleportCroodX = obj.GetIntProperty("f_teleportCroodX", 0);
        teleportCroodY = obj.GetIntProperty("f_teleportCroodY", 0);
    }
}