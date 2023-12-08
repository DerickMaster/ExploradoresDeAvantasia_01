using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge_10
{
    public class LettersManager : MonoBehaviour
    {
        [SerializeField] private Mesh[] lettersMeshes;

        private MeshFilter[] slotsMeshFilters;
        private Dictionary<char, Mesh> lettersMeshDict;
        readonly string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private void Start()
        {
            lettersMeshDict = new Dictionary<char, Mesh>();
            for (int i = 0; i < lettersMeshes.Length; i++)
            {
                lettersMeshDict.Add(letters[i], lettersMeshes[i]);
            }

            slotsMeshFilters = GetComponentsInChildren<MeshFilter>();
            SetLetters(letters.ToCharArray(0, 6));
        }

        public void SetLetters(char[] letters)
        {
            for (int i = 0; i < slotsMeshFilters.Length; i++)
            {
                slotsMeshFilters[i].mesh = lettersMeshDict[letters[i]];
            }
        }
    }
}

