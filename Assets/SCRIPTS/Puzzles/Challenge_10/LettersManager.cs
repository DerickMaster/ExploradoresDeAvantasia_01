using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge_10
{
    public class LettersManager : MonoBehaviour
    {
        private MeshFilter[] slotsMeshFilers;
        readonly string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        [SerializeField] private Mesh[] lettersMeshes;
        private Dictionary<char, Mesh> lettersMeshDict;

        private void Start()
        {
            lettersMeshDict = new Dictionary<char, Mesh>();
            for (int i = 0; i < lettersMeshes.Length; i++)
            {
                lettersMeshDict.Add(letters[i], lettersMeshes[i]);
            }
        }


        public void SetLetters(string[] letters)
        {

        }

        public void SetLetter(char[] letters)
        {

        }
    }
}

