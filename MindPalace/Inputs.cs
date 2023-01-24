using LittleFirstPerson;
using MelonLoader;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace MindPalace
{
    public static class Inputs
    {

        public static void ProcessInputs()
        {
            if (!LittleFirstPersonMain.fpsActive) return;

            if (Input.GetKeyDown(KeyCode.H))
            {
                Notes.CreateNewNote();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                Notes.OpenNearestNoteWithinRadius(2);
            }
        }



    }
}
