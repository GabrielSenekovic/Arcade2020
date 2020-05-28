using System.Collections;
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
            CharacterIdentity myIdentity;
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
            new Dialog.Line("Hey dude, check out that crab egg! Pass me the ball and see if we can destroy it.", Dialog.Line.CharacterIdentity.P1),
            new Dialog.Line("", Dialog.Line.CharacterIdentity.P2)
        }
    )
    {
    };
}
