using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GXPEngine;

/*
 * Text display in-game
 */
public class TextCanvas : Canvas
{
    string theText;
    int theFontSize;
    int theR;
    int theG;
    int theB;
    EasyDraw theTextCanvas;

    bool transparentBackground = false;
    public TextCanvas(string theText, string theFont, int theFontSize, int width, int height,
        int theR, int theG, int theB, bool transparentBackground) : base(width, height)
    {
        this.theText = theText;
        this.theFontSize = theFontSize;

        theTextCanvas = new EasyDraw(width, height);

        this.theR = theR;
        this.theG = theG;
        this.theB = theB;


        Font theTextFont = Utils.LoadFont(theFont, this.theFontSize);
        theTextCanvas.TextFont(theTextFont);
        theTextCanvas.TextAlign(CenterMode.Center, CenterMode.Center);
        AddChild(theTextCanvas);
        this.transparentBackground = transparentBackground;
    }

    void Update()
    {
        if (transparentBackground)
        {
            theTextCanvas.Clear(0, 0, 0, 100);
        }
        theTextCanvas.Fill(theR, theG, theB);
        theTextCanvas.Text(theText, width / 2, height / 2);
    }
    public void SetPoint(float x, float y)
    {
        theTextCanvas.x = x;
        theTextCanvas.y = y;
    }

    public void ChangeText(string theText)
    {
        this.theText = theText;
    }

}