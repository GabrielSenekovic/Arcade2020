﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manuscript
{
    public struct Dialog
    {
        public struct Line
        {
            public enum CharacterIdentity
            {
                P1 = 0,
                P2 = 1,
                ALCHEMIST = 2
            }

            public Line(string line, CharacterIdentity identity)
            {
                myLine = line;
                myIdentity = identity;
            }
            public string myLine;
            public CharacterIdentity myIdentity;
        }
        public Dialog(List<Line> lines)
        {
            myLines = lines;
        }
        public List<Line> myLines;

    }
    public Dialog tutorialDialog = new Dialog
    (
        new List<Dialog.Line>()
        {
            new Dialog.Line("Pass me the ball with enter, bro!", Dialog.Line.CharacterIdentity.P1),
            new Dialog.Line("Whoa that blob thing burst, dude!", Dialog.Line.CharacterIdentity.P2),
            new Dialog.Line("Pass the ball back with space, dude!", Dialog.Line.CharacterIdentity.P2),
            new Dialog.Line("Come over here with right ctrl, bro!", Dialog.Line.CharacterIdentity.P1),
            new Dialog.Line("You dash with left ctrl, dude!", Dialog.Line.CharacterIdentity.P2),
            new Dialog.Line("You will have to help me with that door, bro.", Dialog.Line.CharacterIdentity.P1),
            new Dialog.Line("Yeah, I will stand next to you when you are there to open it.", Dialog.Line.CharacterIdentity.P2),
            new Dialog.Line("And if you get too tired to go on I will stand next to you and boost your spirit!", Dialog.Line.CharacterIdentity.P2),
            new Dialog.Line("I will do the same for you, bro!", Dialog.Line.CharacterIdentity.P1)
        }
    )
    {
    };
}
