using UnityEngine;

public class EndGameCaller : MonoBehaviour
{

    private void Start()
    {
        CanvasBehaviour.Instance.ActivateEndgameScreen();
    }

}
