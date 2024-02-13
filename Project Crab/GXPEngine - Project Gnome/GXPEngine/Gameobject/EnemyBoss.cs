using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
class EnemyBoss : Character
{
    protected bool hasGravty;

    public EnemyBoss(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        hasGravty = obj.GetBoolProperty("f_hasGravity", true);
        AddBehaviorByName(obj.GetStringProperty("b_behavior1Name", ""));

        if (obj.GetStringProperty("b_behavior2Name", "") != "")
        {
            Console.WriteLine("added behavior");
            AddBehaviorByName(obj.GetStringProperty("b_behavior2Name", ""));
        }
    }

    protected override void Update()
    {
        base.Update();

        if (hasGravty)
        {
            VerticalMovement(false);
        }
    }

    public override int CheckIsColliding()
    {
        GameObject[] collisions = GetCollisions();

        foreach (GameObject theCollision in collisions)
        {
            if (theCollision is Tile)
            {
                return 1;
            }

            if (theCollision is WaypointJump)
            {
                return 2;
            }

            if (theCollision is Player)
            {
                return 1;
            }

            if (theCollision is Enemy)
            {
                return 1;
            }
        }
        return 0;
    }

}