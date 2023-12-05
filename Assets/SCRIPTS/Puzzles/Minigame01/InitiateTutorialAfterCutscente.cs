using UnityEngine;
using UnityEngine.Playables;

public class InitiateTutorialAfterCutscente : MonoBehaviour
{
    [SerializeField] PlayableDirector cutscene;
    [SerializeField] TriggerMoveTutorial tutorial;

    private void Start()
    {
        cutscene.stopped += TriggerTutorial;
    }

    private void TriggerTutorial(PlayableDirector obj)
    {
        tutorial.enabled = true;
    }
}
