using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

//same as a sprtie class, but this class allows a sprite's image, scaleX, scaleY, collision to be customized (from Tiled).
public class SpriteCustom : Sprite
{
    
    public SpriteCustom(string theImageName, float scaleX, float scaleY, bool textureKeepInCache, bool hasCollision)
        : base (theImageName, textureKeepInCache, hasCollision)
    {
        this.scaleX = scaleX;
        this.scaleY = scaleY;
    }
    

    public SpriteCustom(string theImageName, TiledObject obj= null) : base(theImageName, obj.GetBoolProperty("i_textureKeepInCache", false),
        obj.GetBoolProperty("p_hasCollision", false))
    {
        if (obj.GetBoolProperty("p_hasCollision", false) == true && obj.GetBoolProperty("p_isTrigger", false))
        {
            collider.isTrigger = true;
        }
    }

    public int CheckIsColliding()
    {
        GameObject[] collisions = GetCollisions();

        foreach (GameObject theCollision in collisions)
        {
            if (theCollision is Tile)
            {
                Tile theTile = (Tile)theCollision;

                if (theTile.GetisDeadly() && this is Player)
                {
                    return 5;
                }

                else
                {
                    return 1;
                }
            }

            if (theCollision is WaypointJump)
            {
                return 2;
            }

            if (theCollision is Player)
            {
                return 3;
            }

            if (theCollision is Enemy)
            {
                return 4;
            }
        }
        return 0;
    }
}


