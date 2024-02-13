using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

/*
 * A bullet object. Intended to be used with shooter behavior. Will keep moving until  (will destroy itself) 
 * a certain distance or collide with some things.
 */
class Bullet : SpriteCustom
{
    float moveSpeedX;
    float moveSpeedY;
    int collisionTimer = Time.time;
    int bulletMoveTime;
    int theBulletDistance;

    float distanceOriginX; //the crood the bullet spawns at. To help calculating the total distance traveled
    float distanceOriginY;

    public Bullet(string theImageName, float theImageScale, float moveSpeedX, float moveSpeedY,
        int bulletMoveTime, int theBulletDistance, bool textureKeepInCache, bool hasCollision) :
        base(theImageName, theImageScale, theImageScale, textureKeepInCache, hasCollision)
    {
        this.moveSpeedX = moveSpeedX;
        this.moveSpeedY = moveSpeedY;
        this.bulletMoveTime = bulletMoveTime;
        this.theBulletDistance = theBulletDistance;
    }

    public void updateOriginPoint()
    {
        distanceOriginX = x;
        distanceOriginY = y;
    }

    void Update()
    {
        if (Time.time - collisionTimer > bulletMoveTime)
        {
            x += moveSpeedX;
            y += moveSpeedY;

            int collider = CheckIsColliding();

            //Collide with wall
            if (collider == 1)
            {
                LateDestroy();
            }

            //Collide with player
            if (collider == 3)
            {
                GameData.playerHealth--;
                LateDestroy();
            }

            //If distance traveled is too long
            if (Mathf.Sqrt((x - distanceOriginX) * (x - distanceOriginX) + (y - distanceOriginY) * (y - distanceOriginY)) >= theBulletDistance)
            {
                LateDestroy();
            }

            collisionTimer = Time.time;
        }
    }
}

