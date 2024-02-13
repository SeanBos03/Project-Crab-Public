using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

/*
 * Button for the menu
 */
public class Button : AnimationSpriteCustom
{
    string theAction;
    TiledObject obj;
    public Button(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        visible = obj.GetBoolProperty("f_visible", true);
        theAction = obj.GetStringProperty("f_theAction1", "");
        id = obj.GetStringProperty("f_theID", "");
        this.obj = obj;
        if (id == "levelSelect")
        {
            alpha = 0.5f;
        }

        if (alpha != 1)
        {
            switch (id)
            {
                case "levelSelect":
                    if (obj.GetIntProperty("parameter1Int", 0) <= GameData.levelCleared + 1)
                    {
                        alpha = 1f;
                    }
                    break;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0))
        {
            if (CustomUtil.CheckPointWithRect(new Vector2(x, y), width, height, new Vector2(Input.mouseX, Input.mouseY)))
            {
                switch (theAction)
                {
                    case "loadMenu":
                            GameData.theLevelName = obj.GetStringProperty("parameter1String", "");
                            GameData.isMenu = true;
                            GameData.playerDead = true;

                        break;
                    case "startGame":
                            if (obj.GetIntProperty("parameter1Int", 0) <= GameData.levelCleared + 1)
                            {
                                GameData.theLevelName = obj.GetStringProperty("parameter1String", "");
                                GameData.lastLevelPlayed = obj.GetStringProperty("parameter1String", "");
                                GameData.isMenu = false;
                                GameData.playerDead = true;
                            }

                        break;

                    case "loadNextLevel":
                        switch (GameData.lastLevelPlayed)
                        {
                            case "map1.tmx":
                                GameData.theLevelName = "map2.tmx";
                                break;
                            case "map2.tmx":
                                GameData.theLevelName = "map3.tmx";
                                break;
                        }
                        GameData.isMenu = false;
                        GameData.playerDead = true;
                        return;
                }
            }
        }
    }
}