using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorials;
    public bool[] activateds;

    private void Start()
    {
        activateds = new bool[tutorials.Length];
    }

    public void TutorialTriggered(string tutorialName)
    {
        for (int i = 0; i < tutorials.Length; i++)
        {
            if (tutorials[i].name.Equals(tutorialName)) 
            {
                activateds[i] = true;
                return;
            }
        }
    }
}
