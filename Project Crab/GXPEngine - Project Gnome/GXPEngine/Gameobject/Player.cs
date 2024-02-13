using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Managers;
using TiledMapParser;

/*
 * The player character and movements
 */
public class Player : Character
{
    int runStartFrame;
    int runAmountOfFrames;
    int idleStartFrame;
    int idleAmountOfFrames;
    int jumpStartFrame;
    int jumpAmountOfFrames;
    bool isJumpPrev = false;

    bool ableToJump = true;
    int jumpTimer = Time.time;

    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        idleStartFrame = obj.GetIntProperty("a_idleStartFrame", 0);
        idleAmountOfFrames = obj.GetIntProperty("a_idleNumberOfFrames", 0);
        runStartFrame = obj.GetIntProperty("a_runStartFrame", 0);
        runAmountOfFrames = obj.GetIntProperty("a_runNumberOfFrames", 0);
        jumpStartFrame = obj.GetIntProperty("a_jumpStartFrame", 0);
        jumpAmountOfFrames = obj.GetIntProperty("a_jumpNumberOfFrames", 0);
        SetAnimationCycle(idleStartFrame, idleAmountOfFrames);
        GameData.theLevel.AddChild(this);
    }

    protected override void Update()
    {
        base.Update();
        CheckPlayerControl();

        if (ableToJump == false)
        {
            if (Time.time - jumpTimer >= 100)
            {
                ableToJump = true;
            }
        }
    }

    public void SetAnimation(int theAnimation)
    {
        switch (theAnimation)
        {
            case 0:
                SetAnimationCycle(idleStartFrame, idleAmountOfFrames);
                break;
            case 1:
                SetAnimationCycle(jumpStartFrame, jumpAmountOfFrames);
                break;
            default:
                SetAnimationCycle(runStartFrame, runAmountOfFrames);
                break;
        }
    }

    void CheckPlayerControl()
    {
        int collider = CheckIsColliding();

        isMovingHoz = false;

        if (Input.GetKey(Key.RIGHT) || Input.GetKey('D'))
        {
            HozMovement(true);
        }

        if (Input.GetKey(Key.LEFT) || Input.GetKey('A'))
        {
            HozMovement(false);
        }

        if ((Input.GetKey(Key.SPACE) || Input.GetKey('W')) && ableToJump)
        {
            VerticalMovement(true);

            ableToJump = false;
            jumpTimer = Time.time;
        }


        else
        {
            if (collider != 2)
            {
                VerticalMovement(false);
            }

        }

        //If player got stuck into wall (from stomping enemy)
        if (collider == 1)
        {
            y -= 1;
        }

        if (collider == 4)
        {
            AnimationSpriteCustom theEnemy = findCollider("enemy_patrol");

            if (isJumping)
            {
                if (GameData.playerDead != true)
                {
                    if (theEnemy != null)
                    {
                        SoundChannel theSound = new Sound("bugDeath.wav", false, false).Play();
                        theEnemy.LateDestroy();
                    }

                }
            }

            else
            {
                SoundChannel theSound = new Sound("playerDead.wav", false, false).Play();
                GameData.playerDead = true;
                return;
            }
        }
    }

    public override void VerticalMovement(bool hasJumpIntent)
    {
        if (isJumping && isJumping != isJumpPrev)
        {
            SetAnimationCycle(jumpStartFrame, jumpAmountOfFrames);
            isJumpPrev = true;
        }

        if (isJumpPrev != isJumping && !isMovingHoz)
        {
            SetAnimationCycle(idleStartFrame, idleAmountOfFrames);
            isJumpPrev = false;
        }

        if (isMovingHoz && !isJumping)
        { 
            SetAnimationCycle(runStartFrame, runAmountOfFrames);
        }

        if (isMovingHoz && isJumping)
        {
            SetAnimationCycle(jumpStartFrame, jumpAmountOfFrames);
        }

        if (!isMovingHoz && !isJumping)
        {
            SetAnimationCycle(idleStartFrame, idleAmountOfFrames);
        }

        base.VerticalMovement(hasJumpIntent);
    }

}
