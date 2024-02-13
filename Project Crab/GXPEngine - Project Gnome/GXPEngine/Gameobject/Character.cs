using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using GXPEngine.Managers;
using TiledMapParser;

/*
 * All characters (the player and enemies) are inherited from this class. Provide walking and jumping movements, and gravity. 
 */
public class Character : AnimationSpriteCustom
{
    float theSpeed;
    float currentSpeedX;
    float currentSpeedY;
    protected bool canAllowTopJump = false; //Can jump to ceiling and keep jumping into the ceiling if holding jump (not working anymore)
    protected bool isJumping = false;
    float jumpHeightAndSpeed;
    protected bool isMovingHoz = false;
    List<Behavior> behaviorList = new List<Behavior>(); //storing all behaviors (tasks) the character has
    TiledObject obj = null; //Map parser data

    public Character(string theImageName, int columns, int rows, TiledObject obj=null) :
    base(theImageName, columns, rows, obj)
    {
        theSpeed = obj.GetFloatProperty("f_theSpeed", 4); //The moving left and right speed
        jumpHeightAndSpeed = obj.GetFloatProperty("f_jumpHeightAndSpeed", 1); //The jump height
        currentSpeedX = theSpeed;
        currentSpeedY = jumpHeightAndSpeed;
        this.obj = obj;
    }


    //Setting the moving left and right speed
    public void setSpeed(float theSpeed)
    {
        this.theSpeed = theSpeed;
        currentSpeedX = this.theSpeed;
    }

    //Setting the jump height
    public void setJumpHeightAndSpeed(float jumpHeightAndSpeed)
    {
        this.jumpHeightAndSpeed = jumpHeightAndSpeed;
        currentSpeedY = this.jumpHeightAndSpeed;
    }
    protected override void Update()
    {
        base.Update();

        //do all behaviors the character has
        foreach (Behavior b in behaviorList)
        {
            b.Action();
        }
    }

    public void AddBehavior(Behavior behavior)
    {
        behaviorList.Add(behavior);
    }

    public virtual void VerticalMovement(bool hasJumpIntent)
    {
        float oldY = y;

        //When player is on the ground and wants to jump, so start jmup
        if (hasJumpIntent && !isJumping)
        {
            isJumping = true;

            if (this is Player)
            {
                SoundChannel newSound = new Sound("hop.wav", false, false).Play();
            }

            currentSpeedY = -jumpHeightAndSpeed; //CurrentSpeedY helps with gravity and will decrease over time until a certain threshold
        }

        //When player is jumping
        if (isJumping)
        {
            y += currentSpeedY; 
            if (currentSpeedY > -jumpHeightAndSpeed - 10)
            {
                currentSpeedY = currentSpeedY + 0.75f;
                if (currentSpeedY > 30)
                {
                    currentSpeedY = 20;
                }
            }
        }

        //If player isn't jumping, adding more value to y would fix player floating (whening moving left and right)
        else
        {
            currentSpeedY = theSpeed;
            y += jumpHeightAndSpeed;
        }

        int collider = CheckIsColliding();
        
        //If touched by 'deadly' tile
        if (collider == 5)
        {
            if (this is Player)
            {
                SoundChannel theSound = new Sound("playerDead.wav", false, false).Play();
                GameData.playerDead = true;
            }
            collider = 1;
        }

        //Collider with wall or if player touches the boss
        if (collider == 1 || (this is Player && collider == 7))
        {
            y = oldY;
            Collision gravityCollision = MoveUntilCollision(0, 2);

            if (gravityCollision != null)
            {
                if (gravityCollision.normal.y < 0 || canAllowTopJump)
                {
                    isJumping = false;
                }
            }
        }
    }

    public void HozMovement(bool isRight)
    {
        isMovingHoz = true;

        float oldX = x;

        if (isRight)
        {
            x += currentSpeedX;
        }

        else
        {
            x -= currentSpeedX;
        }

        int collider = CheckIsColliding();

        if (collider == 5)
        {
            if (this is Player)
            {
                SoundChannel theSound = new Sound("playerDead.wav", false, false).Play();
                GameData.playerDead = true;
            }
            collider = 1;
        }

        if (collider == 1 || (this is Player && collider == 7))
        {
            x = oldX;
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public void AddBehaviorByName(string theName)
    {
        switch (theName)
        {
            case "patrol":
                BehaviorPatrol newBehavior = new BehaviorPatrol(this, obj.GetIntProperty("f_idleActionTimerMin"), obj.GetIntProperty("f_idleActionTimerMax"),
                    obj.GetIntProperty("f_DistanceRightMin"), obj.GetIntProperty("f_DistanceRightMax"), obj.GetIntProperty("f_DistanceLeftMin"),
                    obj.GetIntProperty("f_DistanceLeftMax"), obj.GetIntProperty("f_walkActionPerformTime"));

                AddBehavior(newBehavior);
                break;
            case "shooter":
                BehaviorShooter newBehaviorShooter = new BehaviorShooter(GameData.thePlayer, this, obj.GetIntProperty("f_bulletDistance"), obj.GetIntProperty("f_shooterCooldownMin"),
                    obj.GetIntProperty("f_shooterCooldownMax"), obj.GetIntProperty("f_bulletSpeed"), obj.GetFloatProperty("f_bulletImageScale"),
                    obj.GetStringProperty("f_bulletFileName"), obj.GetIntProperty("f_bulletMoveTime"), obj.GetIntProperty("f_bulletOffSetX"),
                    obj.GetIntProperty("f_bulletOffSetY"), obj.GetIntProperty("f_shooterDetectDistance"));
                AddBehavior(newBehaviorShooter);
                break;
        }
    }

}
