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

            Line(string line, CharacterIdentity identity)
            {
                myLine = line;
                myIdentity = identity;
            }
            string myLine;
            CharacterIdentity myIdentity;
        }
        Dialog(List<Line> lines)
        {
            myLines = lines;
        }
        List<Line> myLines;

    }
    public List<string> dialogs = new List<string>()
    {
        "Hey dude, check out that crab egg! Pass me the ball and see if we can destroy it."
    };
}
