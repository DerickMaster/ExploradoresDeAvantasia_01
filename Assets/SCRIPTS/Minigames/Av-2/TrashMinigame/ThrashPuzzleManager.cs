using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ThrashPuzzleManager : MonoBehaviour
{
    public GameObject slotsParent;

    [SerializeField] private PlayableDirector introCinematic;
    [SerializeField] private int minRange;
    [SerializeField] private int maxRange;
    private int bagsAmount;
    private System.Random rng;

    public GameObject[] thrashPrefab;

    private void Start()
    {
        rng = new System.Random();
        Transform[] slots = slotsParent.GetComponentsInChildren<Transform>();
        bagsAmount = Random.Range(minRange, maxRange + 1);
        int[] bagsType = GenerateRandomBags();
        rng.Shuffle(slots);
        for (int i = 0; i < bagsAmount; i++)
        {
            Instantiate(thrashPrefab[bagsType[i]], slots[i].position, Quaternion.identity); 
        }
        MainThrashCollectorBehaviour main = GetComponentInChildren<MainThrashCollectorBehaviour>();

        main.GameWon.AddListener(GameWon);
        main.GameLost.AddListener(GameLost);
        main.expectedThrashes = bagsAmount;

        introCinematic.Play();
    }

    private int[] GenerateRandomBags()
    {
        int[] bags = new int[bagsAmount];
        for (int i = 0; i < bags.Length; i++)
        {
            if (i < thrashPrefab.Length) bags[i] = i;
            else bags[i] = Random.Range(0, thrashPrefab.Length);
        }

        rng.Shuffle(bags);
        return bags;
    }

    private void GameWon()
    {
        //CanvasBehaviour.Instance.SetActiveTempText("VITORIA", 2F);
        CanvasBehaviour.Instance.ActivateEndgameScreen();
    }


    private void GameLost()
    {
        //CanvasBehaviour.Instance.SetActiveTempText("AAAAAAAAA ESTOU MORRENDO EM DOR E SOFRIMENTO", 2F);
        CanvasBehaviour.Instance.ActivateEndgameScreen(false, 3f);
    }
}
static class RandomizeArray
{
    public static void Shuffle<T>(this System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}