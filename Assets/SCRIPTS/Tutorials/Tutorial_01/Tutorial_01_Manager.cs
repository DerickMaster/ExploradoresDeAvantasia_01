using UnityEngine.UI;
using UnityEngine;

public class Tutorial_01_Manager : MonoBehaviour
{
    struct TutorialTrigger
    {
        GameObject Object;
        string Text, MobileText;
    }

    CanvasBehaviour _canvas;
    TutorialTrigger[] Triggers;
    int _tutorialIndex;
    private void Start()
    {
        _canvas = CanvasBehaviour.Instance;
    }
}
