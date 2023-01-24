using System;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace MindPalace
{
    public class Note : MonoBehaviour
    {
        public Note(IntPtr intPtr) : base(intPtr) { }

        public static GameObject notePrefab;

        public static void LoadPrefab()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Note>(true);
            notePrefab = Bundles.mindPalaceBundle.LoadAsset<GameObject>("scroll");
            DontDestroyOnLoad(notePrefab);  
        }

        public static GameObject Instantiate(Vector3 position)
        {
            GameObject noteObject = Instantiate(notePrefab, position, Quaternion.Euler(new Vector3(0, 90 * UnityEngine.Random.Range(0, 1), -45)));
            noteObject.AddComponent<Note>();
            MeshRenderer customRenderer = noteObject.GetComponentInChildren<MeshRenderer>();
            customRenderer.material.shader = Shader.Find("Placemaker/Debris");

            return noteObject;
        }

        public static string GetNoteName(GameObject noteObj)
        {
            Note note = noteObj.GetComponent<Note>();
            return note.GetNoteName();
        }

        public static string GenerateNoteName(Vector3 pos)
        {
            return pos.x + "," + pos.y + "," + pos.z;
        }



        // Persistent variables
        Vector3 originalNotePosition = new Vector3();

        // User Inputs
        public float degreesPerSecond = 15.0f;
        public float amplitude = 0.02f;
        public float frequency = 0.5f;

        // Position Storage Variables
        Vector3 posOffset = new Vector3();
        Vector3 tempPos = new Vector3();

        public string GetNoteName()
        {
            return GenerateNoteName(originalNotePosition);
        }

        // Use this for initialization
        void Start()
        {
            originalNotePosition = transform.position;

            // Store the starting position & rotation of the object
            posOffset = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // Spin object around Y-Axis
            //transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * 2 * frequency) * amplitude;

            transform.position = tempPos;
        }

    }
}
