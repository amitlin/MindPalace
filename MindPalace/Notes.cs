using LittleFirstPerson;
using MelonLoader;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using System.Windows.Forms;
using Graphics = System.Drawing.Graphics;

namespace MindPalace
{
    public static class Notes
    {
        public static string notesDir = @"Mods\MindPalaceNotes";
        public static string screenshotDir = $@"{notesDir}\screenshots";

        public static List<GameObject> notes = new List<GameObject>();

        public static void loadNotesFromStorage()
        {
            string[] noteFiles = Directory.GetFiles(notesDir, "*.md", SearchOption.AllDirectories);
            MelonLogger.Msg($"Identified {noteFiles.Length} note files. Loading...");
            foreach (string noteFile in noteFiles)
            {
                string noteName = Path.GetFileNameWithoutExtension(noteFile);
                MelonLogger.Msg($"Loading note: {noteName}");
                Vector3 noteAsPosition = NoteNameToPosition(noteName);

                notes.Add(Note.Instantiate(noteAsPosition));
                MelonLogger.Msg("Instantiated");
            }
        }

        public static void CreateNewNote()
        {
            Directory.CreateDirectory(notesDir);

            Vector3 position = LittleFirstPersonMain.fpsPlayer.transform.position;
            string noteName = Note.GenerateNoteName(position);
            string notePath = ResolveNoteName(noteName);
            string noteText = GetNoteTemplate(noteName);

            GameObject noteObj = Note.Instantiate(position);
            notes.Add(noteObj);

            SaveScreenshot(noteName);
            File.WriteAllText(notePath, noteText);
            
            openNote(notePath);
        }

        public static string GetNoteTemplate(string noteName)
        {
            string screenshotPath = Path.GetFullPath($@"{screenshotDir}\{noteName}.jpeg");
            return $@"
![Note]({screenshotPath})
# Title
---


";
    }

        public static string ResolveNoteName(string noteName)
        {
            return $@"{notesDir}\{noteName}.md";
        }

        public static void openNote(string notePath)
        {
            MelonLogger.Msg($"Opening note: {notePath}");
            System.Diagnostics.Process.Start(notePath);
        }

        public static void OpenNearestNoteWithinRadius(float radius)
        {
            GameObject nearestNote = null;
            Vector3 currentPos = LittleFirstPersonMain.fpsPlayer.transform.position;
            foreach(GameObject note in notes)
            {
                Vector3 notePos = note.transform.position;
                float distance = Vector3.Distance(currentPos, notePos);
                if (distance > radius) continue;
                if (nearestNote == null) { nearestNote = note; continue; }
                if (distance < Vector3.Distance(currentPos, nearestNote.transform.position)) nearestNote = note; 
            }

            if (nearestNote == null) return;
            string noteName = Note.GetNoteName(nearestNote);
            string notePath = ResolveNoteName(noteName);
            openNote(notePath);
        }


        public static Vector3 NoteNameToPosition(string noteName)
        {
            string[] positionParts = noteName.Split(',');

            float x = float.Parse(positionParts[0], CultureInfo.InvariantCulture.NumberFormat);
            float y = float.Parse(positionParts[1], CultureInfo.InvariantCulture.NumberFormat);
            float z = float.Parse(positionParts[2], CultureInfo.InvariantCulture.NumberFormat);

            Vector3 position = new Vector3(x, y, z);
            return position;
        }

        public static void SaveScreenshot(string noteName)
        {
            Directory.CreateDirectory(screenshotDir);

            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            // Create a bitmap of the appropriate size to receive the full-screen screenshot.
            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight))
            {
                // Draw the screenshot into our bitmap.
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap.Size);
                }

                //Save the screenshot as a Jpg image
                string imageName = $@"{screenshotDir}\{noteName}.jpeg";
                try
                {
                    bitmap.Save(imageName, ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
