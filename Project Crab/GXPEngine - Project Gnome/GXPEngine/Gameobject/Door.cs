using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

/*
 * A gameobject that allows the player to switch
 * from game level to game complete screen
 * 
 * Can be modified to switch to another game level or menu.
 *
 */
public class Door : AnimationSpriteCustom
{
    string theNextLevelName;
    public Door(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        visible = obj.GetBoolProperty("isVisible", false);
        theNextLevelName = obj.GetStringProperty("f_nextLevelName", "map1.tmx");
    }

    public void ChangeLevel()
    {
        GameData.LevelCompleteTime = Math.Round(GameData.LevelCurrentTime / 1000.0m, 2);
        GameData.levelCompleteScore = GameData.levelCurrentScore;
        GameData.CheckNewLevelCleared();
        GameData.lastLevelPlayed = GameData.theLevelName;
        GameData.theLevelName = theNextLevelName;
        GameData.isMenu = true;
        GameData.playerDead = true;
    }
}