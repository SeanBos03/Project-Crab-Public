using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

/*
 * Holds all kind of data all other classes can access and edit
 * thid holds data
 */
public static class GameData
{
    public static string lastLevelPlayed = "map1.tmx"; //determine what level will be loaded
    public static string theLevelName = "mainMenu.tmx"; //determine what level will be loaded
    public static Level theLevel; //the level object itself
    public static bool playerDead = false;
    public static List<WaypointJump> wayPointJumpList = new List<WaypointJump>(); //waypoint that guide enemies to jump

    public static int LevelCurrentTime = 0; //the current time past after the level loaded in milliseconds
    public static decimal LevelCompleteTime = 0; //displaying the time past after the game loaded in second with 2 decimal point accuracy 
    public static int levelCurrentScore;
    public static int levelCompleteScore;
    public static int levelCleared = 0;
    public static int theFPS;

    public static Player thePlayer;
    public static float playerSpeed = 4;
    public static float playerJumpHeightAndSpeed = 12;

    public static int playerMaxHealth = 3;
    public static int playerHealth = 3;

    public static bool isMenu = true;

    public static int[] jumpUpgradeList = new int[] {1,1,1,1,1};
    public static int[] speedUpgradeList = new int[] {1,1,1,1,1};


    public static int bossHealth = 4;

    public static void CheckNewLevelCleared()
    {
        if (levelCleared == 0 && theLevelName == "map1.tmx")
        {
            levelCleared++;
        }

        if (levelCleared == 1 && theLevelName == "map2.tmx")
        {
            levelCleared++;
        }
    }

    public static void ResetLevelData()
    {
        bossHealth = 6;
        LevelCurrentTime = 0;
        levelCurrentScore = 0;
        theLevel = null;
        wayPointJumpList.Clear();
        playerHealth = playerMaxHealth;
        playerDead = false;
        thePlayer = null;
    }

}
