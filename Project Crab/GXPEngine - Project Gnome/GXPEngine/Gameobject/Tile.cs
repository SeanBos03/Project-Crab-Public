using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
public class Tile : AnimationSpriteCustom
{
    /*
     * The 'wall' of the level
     */

    bool isDeadly; //if true, player would die if the player touches this
    public Tile(bool isDeadly, string theImageName, float scaleX, float scaleY, int singleFrameID, int columns, int rows,
        int numberOfFrame, int startFrame, int endFrame, int nextFrameDelay, bool textureKeepInCache, bool hasCollision) :
        base(theImageName, scaleX, scaleY, singleFrameID, columns, rows,
         numberOfFrame, startFrame, endFrame, nextFrameDelay, textureKeepInCache, hasCollision)
    {
        this.isDeadly = isDeadly;
    }

    public bool GetisDeadly()
    {
        return isDeadly;
    }

}