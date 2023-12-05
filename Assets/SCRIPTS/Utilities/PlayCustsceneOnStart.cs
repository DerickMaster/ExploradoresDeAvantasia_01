using UnityEngine;
using UnityEngine.Playables;

public class PlayCustsceneOnStart : MonoBehaviour
{
    [SerializeField] PlayableDirector cutscene;

    private void Start()
    {
        cutscene.Play();
    }
}
