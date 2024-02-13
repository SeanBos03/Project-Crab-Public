using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

/*
 * Makes the npc shoot bullets if detect a choosen target within a certain distance
 */
class BehaviorShooter : Behavior
{
    GameObject theTarget; //The target the npc will shoot at
    int cooldownMax; //Shoot cooldown
    int cooldownMin;
    int theCooldown; //For cooldown calculation
    int shootTimer = Time.time; 
    float bulletSpeed;
    float bulletImageScale;
    string bulletFileName;
    int enemyBulletMoveTime;
    int enemyDetectDistance; //The detection range of the npc
    int bulletDistance; //The distacne the bullet can live. (if bullet total distance moved is over this value, destroy the bullet)
    int offSetX; //offset postion the bullet will spawn at from the npc's center
    int offSetY;
    public BehaviorShooter(Character theTarget, Character theShooter, int bulletDistance, int cooldownMin,
        int cooldownMax, int bulletSpeed, float bulletImageScale, string bulletFileName, int enemyBulletMoveTime, int offSetX,
        int offSetY, int enemyDetectDistance)
        : base(theShooter, 0, 0, 0)
    {
        this.theTarget = theTarget;
        this.bulletImageScale = bulletImageScale;
        this.bulletFileName = bulletFileName;
        this.bulletSpeed = bulletSpeed;
        this.cooldownMax = cooldownMax;
        this.cooldownMin = cooldownMin;
        this.enemyBulletMoveTime = enemyBulletMoveTime;
        this.bulletDistance = bulletDistance;
        this.offSetX = offSetX;
        this.offSetY = offSetY;
        this.enemyDetectDistance = enemyDetectDistance;
        theCooldown = Utils.Random(cooldownMin, cooldownMax + 1); //Deciding the cooldown value
    }
    public override void Action()
    {
        //If no target is provided, default to player. If player is also null, do nothing
        if (theTarget == null)
        {
            theTarget = GameData.thePlayer;
            if (theTarget == null)
            {
                return;
            }
        }

        //Shoot cooldown
        if (Time.time - shootTimer >= theCooldown)
        {

            //Calculating the distance
            float length = Mathf.Sqrt((theTarget.x - theOwner.x) * (theTarget.x - theOwner.x) + (theTarget.y - theOwner.y) * (theTarget.y - theOwner.y));

            //Prevent dividing by 0
            if (length <= 0.1f)
            {
                return;
            }

            //If npc detects target (target in the distance)
            if (enemyDetectDistance >= length)
            {
                //spawn bullet
                Bullet bullet = new Bullet(bulletFileName, bulletImageScale, (theTarget.x - theOwner.x) * (bulletSpeed / length), 
                    (theTarget.y - theOwner.y) * (bulletSpeed / length),
                    enemyBulletMoveTime, bulletDistance, false, true);

                bullet.SetXY((theOwner.x + theOwner.GetWidth() / 2) + offSetX, (theOwner.y + theOwner.GetHeight() / 2) + offSetY);
                bullet.updateOriginPoint();
                theOwner.parent.AddChild(bullet);

                SoundChannel bulletSound = new Sound("pop1.mp3", false, false).Play();
                bulletSound = theOwner.TweakSound(bulletSound, true, true, bullet, 44100);
            }

            shootTimer = Time.time;
            theCooldown = Utils.Random(cooldownMin, cooldownMax + 1);
        }
    }
    
}
