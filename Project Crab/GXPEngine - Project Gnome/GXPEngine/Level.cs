using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Messaging;
using System.Text;
using GXPEngine;
using TiledMapParser;

/*
 * The level Gameobject. For generating the game itself as a menu screen or a game level
 * The game should only have one level
 */
public class Level : GameObject
{
    TiledLoader loader;
    Player thePlayer;

    //Determine the position the player will be displayed in the game camera
    float boundaryValueX; //Should be width / 2 to display the player at the center of the screen
    float boundaryValueY; //Should be height / 2 to display the player at the center of the screen

    Dictionary<string, TextCanvas> textCanvasListHash = new Dictionary<string, TextCanvas>();
    int pickUpCheckTimer;
    int levelCompleteTimer = Time.time;
    const int PICKUPCHECKTIME = 15;
    List<PickUpCoin> coinList = new List<PickUpCoin>();
    List<Door> doorList = new List<Door>();
    List<Teleporter> teleporterList = new List<Teleporter>();
    List<PickupHealth> pickupHealthList = new List<PickupHealth>();
    List<PickupSpeedUpgrade> pickupSpeedUpgradeList = new List<PickupSpeedUpgrade>();
    List<PickupJumpSpeedUpgrade> pickupJumpSpeedUpgradeList = new List<PickupJumpSpeedUpgrade>();
    List<BossPlant> bossPlantList = new List<BossPlant>();


    List<TriggerAction> triggerActionList = new List<TriggerAction>();
    public Level(string theMapfileName)
    {
        Map mapData = MapParser.ReadMap(theMapfileName);
        GameData.theLevel = this;
        loader = new TiledLoader(theMapfileName);

        //If the game lvel is a menu
        if (GameData.isMenu)
        {
            //Automatically generates the game objects
            loader.autoInstance = true;
            loader.rootObject = this;
            loader.LoadImageLayers();
            loader.LoadObjectGroups(0);
            loader.LoadObjectGroups(1);
            loader.LoadObjectGroups(2);
            //Manually generates some of the game objects
            SpawnObjects(mapData);
        }

        //If the game level is not a menu
        else
        {
            //Manually generates the tile layers.
            CreateTile(mapData, 0); //Create the walls
            CreateTile(mapData, 1); //Create background
            CreateTile(mapData, 2); //Create traps
            //Automatically generates the game objects
            loader.autoInstance = true;
            loader.rootObject = this;
            loader.LoadImageLayers();
            loader.LoadObjectGroups(0);

            //A game level should have a player, so find the player and allowing the program to reference it via gamedata
            thePlayer = FindObjectOfType<Player>();
            GameData.thePlayer = thePlayer;
            thePlayer.setSpeed(GameData.playerSpeed);
            thePlayer.setJumpHeightAndSpeed(GameData.playerJumpHeightAndSpeed);
            //increase player speed



            //Extracting all the wayPointJump objects created so enemy can detect them
            foreach (WaypointJump theWayPoint in FindObjectsOfType<WaypointJump>())
            {
                GameData.wayPointJumpList.Add(theWayPoint);
            }


            //Extracting all the coins so player can detect them
            foreach (PickUpCoin theCoin in FindObjectsOfType<PickUpCoin>())
            {
                coinList.Add(theCoin);
            }

            //Extracting all the bossplant objects created so the player can detect them
            foreach (BossPlant theBossPlant in FindObjectsOfType<BossPlant>())
            {
                bossPlantList.Add(theBossPlant);
            }

            //Extract PickupSpeedUpgrade
            foreach (PickupSpeedUpgrade thePickupSpeedUpgrade in FindObjectsOfType<PickupSpeedUpgrade>())
            {
                if (GameData.speedUpgradeList[thePickupSpeedUpgrade.theIndex] == 1)
                {
                    pickupSpeedUpgradeList.Add(thePickupSpeedUpgrade);
                }

                else
                {
                    thePickupSpeedUpgrade.Destroy();
                }
            }

            //Extract PickupJumpSpeedUpgrade
            foreach (PickupJumpSpeedUpgrade thePickupJumpSpeedUpgrade in FindObjectsOfType<PickupJumpSpeedUpgrade>())
            {
                if (GameData.jumpUpgradeList[thePickupJumpSpeedUpgrade.theIndex] == 1)
                {
                    pickupJumpSpeedUpgradeList.Add(thePickupJumpSpeedUpgrade);
                }

                else
                {
                    thePickupJumpSpeedUpgrade.Destroy();
                }
            }

            //Extract PickupHealth
            foreach (PickupHealth theHealthPickup in FindObjectsOfType<PickupHealth>())
            {
                pickupHealthList.Add(theHealthPickup);
            }

            //Extracting all the Door objects created so the player can detect them
            foreach (Door theDoor in FindObjectsOfType<Door>())
            {
                doorList.Add(theDoor);
            }

            //Extracting all the teleporter objects
            foreach (Teleporter theTeleporter in FindObjectsOfType<Teleporter>())
            {
                if (theTeleporter.groupID == "teleporterPlayer")
                {
                    theTeleporter.theOwner = thePlayer;
                }
                teleporterList.Add(theTeleporter);
            }


            //Manually create some of the game objects to make things working
            SpawnObjects(mapData);
        }

        //Setting up the camera boundary (player at center for these values)
        boundaryValueX = game.width / 2;
        boundaryValueY = game.height / 2;


    }

    //Some game objects can't be automacally generate (sometime program needs their reference), so
    //manually generate some of the gameobjects. 
    //If is a menu level, extract layer 3, if is not a menu level, extract layer 1
    void SpawnObjects(Map mapData)
    {
        //Check data is not empty
        if (mapData.ObjectGroups == null || mapData.ObjectGroups.Length == 0)
        {
            return;
        }

        ObjectGroup objectGroup; //Holding the list of gameobject we will extract

        //A menu level has a different layer for the gameobjects that need to be manually generated
        if (GameData.isMenu)
        {
            objectGroup = mapData.ObjectGroups[3];
        }

        else
        {
            objectGroup = mapData.ObjectGroups[1];
        }

        //Check if the selected layer has objects
        if (objectGroup.Objects == null || objectGroup.Objects.Length == 0)
        {
            return;
        }

        //Extracting the game objects layer
        foreach (TiledObject theObject in objectGroup.Objects)
        {
            Sprite nextGameObject = null;
            switch (theObject.Name)
            {
                //Detect is it's a text canvas. A text display
                case "TextCanvas":
                    //Extracting properties from what's exported from tiled
                    TextCanvas theTextCanvas = new TextCanvas(theObject.GetStringProperty("theTextContent", ""),
                        theObject.GetStringProperty("theTextFont", ""), theObject.GetIntProperty("theTextSize", 0),
                         theObject.GetIntProperty("theCanvasWidth", 0), theObject.GetIntProperty("theCanvasHeight", 0),
                         theObject.GetIntProperty("theTextColorR", 0), theObject.GetIntProperty("theTextColorG", 0),
                         theObject.GetIntProperty("theTextColorB", 0), theObject.GetBoolProperty("transparentBackground", false));

                    theTextCanvas.visible = theObject.GetBoolProperty("visibleAtStart", false);

                    //Detecting if the text canvas is one of the specific text canvases
                    switch (theObject.GetStringProperty("f_displayID", ""))
                    {
                        case "textCavas_displayTime":
                            theTextCanvas.ChangeText(GameData.LevelCompleteTime.ToString());
                            break;
                        case "textCavas_displayScore":
                            theTextCanvas.ChangeText(GameData.levelCompleteScore.ToString());
                            break;
                    }

                    textCanvasListHash.Add(theObject.GetStringProperty("theTextCanvasID", ""), theTextCanvas);
                    nextGameObject = theTextCanvas;
                    break;
            }

            //Adjusting the position if a gameobject is found and add it to game
            if (nextGameObject != null)
            {
                nextGameObject.x = theObject.X;
                nextGameObject.y = theObject.Y;
                AddChild(nextGameObject);
            }
        }

        /* 
         * Some gameobjects must be loaded after some other gameobject to make things work properly,
         * so repeat the extraction process again for some gameobjects
         */

        foreach (TiledObject theObject in objectGroup.Objects)
        {
            Sprite nextGameObject = null;
            switch (theObject.Name)
            {
                //Trigger region
                case "RegionActivateAtOnce":

                    RegionActivateAtOnce theRegion = new RegionActivateAtOnce(thePlayer, theObject.GetBoolProperty("triggerAction1Parameter1Bool", false),
                        theObject.GetIntProperty("i_scaleX", 1), theObject.GetIntProperty("i_scaleY", 1), theObject.GetIntProperty("f_amountOfTimes", -1));

                    switch (theObject.GetStringProperty("triggerActionName1", ""))
                    {
                        case "enableDisableGameObjectRender":
                            theRegion.AddTriggerAction(new TriggerActionVisbility(textCanvasListHash[theObject.GetStringProperty("triggerAction1Parameter2String", "")],
                                false));
                            break;
                    }

                    nextGameObject = theRegion;
                    break;
            }

            if (nextGameObject != null)
            {
                nextGameObject.x = theObject.X;
                nextGameObject.y = theObject.Y;
                AddChild(nextGameObject);
            }
        }
    }

    void Update()
    {
        //Counting how much time based since the level started in milliseconds

        GameData.LevelCurrentTime = Time.time - levelCompleteTimer;

        //Use camera if player is found
        if (thePlayer != null)
        {
            UseCamera();
        }

        //Check if player touches a coin, enters a door, and other things. With some cooldown.
        if (Time.time - pickUpCheckTimer >= PICKUPCHECKTIME)
        {

            CheckPlayerGetsCoin();
            pickUpCheckTimer = Time.time;
            CheckDoors();
            CheckTeleporters();
            CheckText(); //Updating the time passed and score got to game data
            CheckPickupHealth();
            CheckSpeedUpgradePickup();
            CheckJumpSpeedUpgrade();
            CheckBossPlant();
        }

        CheckTriggerAction();
    }


    //Sets the game area player can look. AKA the game camera
    //Can set how far right and down player can see. (left stops at x < 0, top stops at y < 0)
    void UseCamera()
    {

        //first determine if the camera moves, then determine the max distance the camera can move
        //handling player moving right
        if (thePlayer.x + x > boundaryValueX && x > -1 * ((game.width * 6) - 800))
        {
            x = boundaryValueX - thePlayer.x;
        }

        //handling player moving left
        if (thePlayer.x + x < game.width - boundaryValueX && x < 0)
        {
            x = game.width - boundaryValueX - thePlayer.x;
        }

        //handling player moving up
        if (thePlayer.y + y < game.height - boundaryValueY && y < 0)
        {
            y = game.height - boundaryValueY - thePlayer.y;
        }

        //handling player moving down
        if (thePlayer.y + y > boundaryValueY && y > -1 - (game.height * 2) - 100)
        {
            y = boundaryValueY - thePlayer.y;
        }
    }


    //Spawns all the Tiles of the level
    void CreateTile(Map mapData, int theLayer)
    {
        //Check if map data is not empty
        if (mapData.Layers == null || mapData.Layers.Length == 0)
        {
            return;
        }
        Layer layer = mapData.Layers[theLayer];


        //Helps to render all the background (layer 1) sprites in one call.
        SpriteBatch background = new SpriteBatch();
        AddChild(background);

        short[,] tileNumbers = layer.GetTileArray(); //holding the tile data

        //Generating the tiles depends on the map data.
        //Extracting the 2d array extracted from the map data. Each number represent a specific tile
        for (int i = 0; i < layer.Height; i++)
        {
            for (int j = 0; j < layer.Width; j++)
            {
                int theTileNumber = tileNumbers[j, i]; //extracting the tile number
                TileSet theTilesSet = mapData.GetTileSet(theTileNumber); //what tile set the number comes from

                //A number with value 0 means no tile, so ignore number 0
                if (theTileNumber != 0)
                {
                    Tile theTile; //the tile object to be added to the game level

                    /*
                     * Determining what type of tile the tile object will be based on the tile layer.
                     * Layer 0 represents wall tiles, layer 1 represents background tiles, layer 2 represent wall tiles that player will die if touched
                     */

                    // A wall tile. isDeadly == false. collision on
                    if (theLayer == 0)
                    {
                        theTile = new Tile(false, theTilesSet.Image.FileName, 1, 1, theTileNumber - theTilesSet.FirstGId,
                            theTilesSet.Columns, theTilesSet.Rows, -1, 1, 1, 10, false, true);
                        theTile.x = j * theTile.width;
                        theTile.y = i * theTile.height;
                        AddChild(theTile);

                    }

                    //Background. isDeadly == false. collision off
                    else if (theLayer == 1)
                    {
                        theTile = new Tile(false, theTilesSet.Image.FileName, 1, 1, theTileNumber - theTilesSet.FirstGId,
                            theTilesSet.Columns, theTilesSet.Rows, -1, 1, 1, 10, false, false);
                        theTile.x = j * theTile.width;
                        theTile.y = i * theTile.height;
                        background.AddChild(theTile);
                    }


                    //A wall tile with isDeadly == false. collision on
                    else if (theLayer == 2)
                    {
                        theTile = new Tile(true, theTilesSet.Image.FileName, 1, 1, theTileNumber - theTilesSet.FirstGId,
                            theTilesSet.Columns, theTilesSet.Rows, -1, 1, 1, 10, false, true);
                        theTile.SetFrame(theTileNumber - theTilesSet.FirstGId);
                        theTile.x = j * theTile.width;
                        theTile.y = i * theTile.height;
                        AddChild(theTile);
                    }
                }
            }
        }
        background.Freeze(); //Freeze all the background tiles by destroying the sprite and their collider. Creating better performance
    }

    //Check if the player intersects with one of the coin(s), increase score is so
    void CheckPlayerGetsCoin()
    {
        foreach (PickUpCoin thePickup in coinList)
        {
            if (CustomUtil.IntersectsSpriteCustomAndAnimationSpriteCustom(thePickup, thePlayer))
            {
                GameData.levelCurrentScore += thePickup.GetScore();
                thePickup.LateDestroy();
                coinList.Remove(thePickup);
                break;
            }
        }
    }

    //Check if the player intersects with one of the door(s), change level if so 
    void CheckDoors()
    {
        foreach (Door theDoor in doorList)
        {
            if (CustomUtil.IntersectsSpriteCustomAndAnimationSpriteCustom(theDoor, thePlayer))
            {
                thePlayer.Move(10000, 100000);
                theDoor.ChangeLevel();
                break;
            }
        }
    }

    //Check if the player intersects with one of the teleporter(s), move player position if so
    void CheckTeleporters()
    {
        foreach (Teleporter theTeleporter in teleporterList)
        {
            if (CustomUtil.IntersectsSpriteCustomAndAnimationSpriteCustom(theTeleporter, thePlayer))
            {
                switch (theTeleporter.theOwner.id)
                {
                    case "Player":
                        thePlayer.SetXY(theTeleporter.teleportCroodX, theTeleporter.teleportCroodY);
                        break;
                }
            }
        }
    }

    //Check if the player intersects with one of the speed upgrades (blue book), increase player speed if so
    void CheckSpeedUpgradePickup()
    {
        for (int i = 0; i < pickupSpeedUpgradeList.Count; i++)
        {
            if (CustomUtil.IntersectsSpriteCustomAndAnimationSpriteCustom(pickupSpeedUpgradeList[i], thePlayer))
            {
                //speedUpgradeList tracks if the upgrade is picked up already
                if (GameData.speedUpgradeList[pickupSpeedUpgradeList[i].theIndex] == 1)
                {
                    GameData.speedUpgradeList[pickupSpeedUpgradeList[i].theIndex] = 0;
                    GameData.playerSpeed += pickupSpeedUpgradeList[i].theAmount;
                    thePlayer.setSpeed(GameData.playerSpeed);

                    if (textCanvasListHash.ContainsKey(pickupSpeedUpgradeList[i].theTextDisplay) == true)
                    {
                        triggerActionList.Add(new TriggerActionVisbilityTimed(textCanvasListHash[pickupSpeedUpgradeList[i].theTextDisplay],
                            false, 3000));
                    }

                    pickupSpeedUpgradeList[i].Destroy();
                    pickupSpeedUpgradeList.RemoveAt(i);
                }
            }
        }
    }

    //Check if the player intersects with one of the jump height upgrades (brown book), increase player jump height if so
    void CheckJumpSpeedUpgrade()
    {
        for (int i = 0; i < pickupJumpSpeedUpgradeList.Count; i++)
        {
            if (CustomUtil.IntersectsSpriteCustomAndAnimationSpriteCustom(pickupJumpSpeedUpgradeList[i], thePlayer))
            {
                //jumpUpgradeList tracks if the upgrade is picked up already
                if (GameData.jumpUpgradeList[pickupJumpSpeedUpgradeList[i].theIndex] == 1)
                {
                    GameData.jumpUpgradeList[pickupJumpSpeedUpgradeList[i].theIndex] = 0;
                    GameData.playerJumpHeightAndSpeed += pickupJumpSpeedUpgradeList[i].theAmount;
                    thePlayer.setJumpHeightAndSpeed(GameData.playerJumpHeightAndSpeed);

                    if (textCanvasListHash.ContainsKey(pickupJumpSpeedUpgradeList[i].theTextDisplay) == true)
                    {
                        Console.WriteLine("add text");
                        triggerActionList.Add(new TriggerActionVisbilityTimed(textCanvasListHash[pickupJumpSpeedUpgradeList[i].theTextDisplay],
                            false, 3000));
                    }

                    pickupJumpSpeedUpgradeList[i].Destroy();
                    pickupJumpSpeedUpgradeList.RemoveAt(i);
                }
            }
        }
    }

    //Check if the player intersects with one of the health pickups, increase player health if player health is not at max
    void CheckPickupHealth()
    {
        for (int i = 0; i < pickupHealthList.Count; i++)
        {

            if (CustomUtil.IntersectsSpriteCustomAndAnimationSpriteCustom(pickupHealthList[i], thePlayer) &&
                (GameData.playerHealth < GameData.playerMaxHealth))
            {
                pickupHealthList[i].GetHealth();
                pickupHealthList[i].Destroy();
                pickupHealthList.RemoveAt(i);
            }
        }
    }

    //Updating the time passed and score to game data
    void CheckText()
    {
        //Update the time
        if (textCanvasListHash.ContainsKey("textCavas_displayTime") == true)
        {
            textCanvasListHash["textCavas_displayTime"].ChangeText(GameData.LevelCompleteTime.ToString());
            textCanvasListHash["textCavas_displayTime"].visible = true;
        }

        //Update the score
        if (textCanvasListHash.ContainsKey("textCavas_displayScore") == true)
        {
            textCanvasListHash["textCavas_displayScore"].ChangeText(GameData.levelCompleteScore.ToString());
        }
    }

    void CheckTriggerAction()
    {
        foreach (TriggerAction theTriggerAction in triggerActionList)
        {
            if (theTriggerAction.triggerActionID == "TriggerActionVisbilityTimed")
            {
                theTriggerAction.Action();
                if (theTriggerAction.finished)
                {
                    triggerActionList.Remove(theTriggerAction);
                    break;
                }
            }
        }
    }

    //This is for level 3 boss fight only
    //Check if the player intersects with one of the bossplants, decrease bossHealth if so
    //Boss health checks the amount of plants collected. Upon level reset, bossHealth = 5
    void CheckBossPlant()
    {
        for (int i = 0; i < bossPlantList.Count; i++)
        {
            if (CustomUtil.IntersectsSpriteCustomAndAnimationSpriteCustom(bossPlantList[i], thePlayer))
            {
                GameData.bossHealth -= bossPlantList[i].amountOfScore;
                bossPlantList[i].Destroy();
                bossPlantList.RemoveAt(i);

                if (GameData.bossHealth <= 0)
                {
                    GameData.LevelCompleteTime = Math.Round(GameData.LevelCurrentTime / 1000.0m, 2);
                    GameData.levelCompleteScore = GameData.levelCurrentScore;
                    GameData.CheckNewLevelCleared();
                    GameData.lastLevelPlayed = GameData.theLevelName;
                    GameData.theLevelName = "levelCompletedMenuEnd.tmx";
                    GameData.isMenu = true;
                    GameData.playerDead = true;
                }
            }
        }
    }
}