using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using System.Drawing;

/*
 * UI to be displayed in-game during a level
 */
class HUD : Canvas
{
    EasyDraw scoreDisplayCanvas;
    EasyDraw fpsDisplay;

    public HUD() : base(100, 100)
    {
        scoreDisplayCanvas = new EasyDraw(100, 50);

        Font theFont = Utils.LoadFont("SwanseaBold-D0ox.ttf", 12);
        scoreDisplayCanvas.TextFont(theFont);
        //(hoz, vert)
        scoreDisplayCanvas.TextAlign(CenterMode.Center, CenterMode.Center);
        scoreDisplayCanvas.Fill(0, 0, 0);
        scoreDisplayCanvas.Text("Score: ", -5, -5);
        scoreDisplayCanvas.Text("Health: ");
        AddChild(scoreDisplayCanvas);
        fpsDisplay = new EasyDraw(300, 50);
        fpsDisplay.SetXY(600, 20);
        AddChild(fpsDisplay);
    }

    void Update()
    {
        scoreDisplayCanvas.Clear(252, 186, 3);
        scoreDisplayCanvas.Text("Score: " + GameData.levelCurrentScore, 50, 15);
        scoreDisplayCanvas.Text("Health: " + GameData.playerHealth, 50, 35);
        //  graphics.Clear(Color.Empty);
        // graphics.DrawString("Score: " + MyGame.theGameData.TheScore, SystemFonts.DefaultFont, Brushes.White, 20, 50);
      //  fpsDisplay.Clear(252, 186, 3);
      //  fpsDisplay.Text("Fps: " + GameData.theFPS);
    }

}
