using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

public class WaypointJump : AnimationSpriteCustom
{
    /*
     * Helps enemy know when to jump
     */

    int direction;     //what direction the enemy must be moving to make them jump. 1 = right, 2 = left, 3 = both left and right
    public WaypointJump(string theImageName, int columns, int rows, TiledObject obj = null) :
    base(theImageName, columns, rows, obj)
    {
        direction = obj.GetIntProperty("f_direction", 0);
        alpha = 0; //make it invisible
    }

    public int GetDirection()
    {
        return direction;
    }
}