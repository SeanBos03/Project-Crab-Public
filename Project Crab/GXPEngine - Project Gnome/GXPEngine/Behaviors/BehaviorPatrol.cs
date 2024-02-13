using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GXPEngine;
using GXPEngine.Core;

/*
 * Makes the npc moved around left and right. Would jump if guided by WaypointJump
 */
public class BehaviorPatrol : Behavior
{
    int rightMaxDistance; //Max distance moving right
    int rightMinDistance;
    int leftMaxDistance; //Max distance moving left
    int leftMinDistance;
    int tempDistance = 0; //For calculation
    int tempDistanceBack; //For calculation
    bool moveRight = false; //determine if NPC should move right by default
    bool isMovingRightSide = false; //For calculation
    int moveStage = 0; //For calculation
    bool isJumping = false; //Determine if NPC should jump (for calculation)

    public BehaviorPatrol(Character theOwner, int actionTimeMin, int actionTimeMax, int rightMinDistance,
        int rightMaxDistance, int leftMinDistance, int leftMaxDistance, int performTime) : base(theOwner, actionTimeMin, actionTimeMax,
            performTime)
    {
        this.rightMaxDistance = rightMaxDistance;
        this.rightMinDistance = rightMinDistance;
        this.leftMaxDistance = leftMaxDistance;
        this.leftMinDistance = leftMinDistance;
    }

    public override void Action()
    {
        base.Action();

        //MoveStage decides what stage of the 'patrol' action the NPC is in

        //Check in all stages
        if (moveStage != -1)
        {
            bool detectWayPointJump = false;
            //Detecting wayPointJump
            foreach (WaypointJump theWayPoint in GameData.wayPointJumpList)
            {
                if (CustomUtil.CheckPointWithRect(new Vector2(theOwner.x, theOwner.y), theOwner.width, theOwner.height,
                    new Vector2(theWayPoint.x, theWayPoint.y)))
                {
                    //Determining to jump or not depending on what direction the waypointJump has. (1 = right, 2 = left, 3 = both directions)
                    if ((theWayPoint.GetDirection() == 1 && isMovingRightSide) || theWayPoint.GetDirection() == 3
                        || (theWayPoint.GetDirection() == 2 && !isMovingRightSide))
                    {
                        detectWayPointJump = true;
                        break;
                    }
                }
            }

            //don't make the NPC jump if the npc is already jumping
            if (detectWayPointJump && isJumping)
            {
                isJumping = false;
            }

            if (detectWayPointJump && !isJumping)
            {

                theOwner.VerticalMovement(true);
                isJumping = true;
            }
        }

        //idle stage
        if (moveStage == 0)
        {
            StartWalk();
        }

        //patrol stage 2
        else if (moveStage == 2)
        {
            if (tempDistanceBack <= 0)
            {
                if (Time.time - theTimer >= Utils.Random(actionTimeMin, actionTimeMax + 1))
                {
                    theTimer = Time.time;
                    moveStage = 0;
                    moveRight = !moveRight;
                    isMovingRightSide = !isMovingRightSide;
                }
            }

            else
            {
                if (Time.time - theTimer >= actionPerformTime)
                {
                    tempDistanceBack--;
                    theOwner.HozMovement(moveRight);
                    theTimer = Time.time;
                }
            }
        }

        //patrol stage 1
        else
        {
            if (tempDistance <= 0)
            {
                if (Time.time - theTimer >= Utils.Random(actionTimeMin, actionTimeMax + 1))
                {
                    theTimer = Time.time;
                    moveStage = 2;
                    moveRight = !moveRight;
                    isMovingRightSide = !isMovingRightSide;
                }
            }

            else
            {
                if (Time.time - theTimer >= actionPerformTime)
                {
                    tempDistance--;
                    theOwner.HozMovement(moveRight);
                    theTimer = Time.time;
                }
            }
        }
    }

    //Reverse the direction moving and make the npc start moving
    void StartWalk()
    {
        moveRight = !moveRight;
        isMovingRightSide = !isMovingRightSide;
        moveStage = 1;

        if (moveRight)
        {
            int rightDistance = Utils.Random(rightMinDistance, rightMaxDistance + 1);
            tempDistance = rightDistance;
            tempDistanceBack = rightDistance;
        }

        else
        {
            int leftDistance = Utils.Random(leftMinDistance, leftMaxDistance + 1);
            tempDistance = leftDistance;
            tempDistanceBack = leftDistance;
        }
    }

}