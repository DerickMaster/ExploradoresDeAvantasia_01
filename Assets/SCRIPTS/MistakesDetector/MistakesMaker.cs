using UnityEngine;

public class MistakesMaker : MonoBehaviour
{
    public IMakeMistakes GetPuzzleReference()
    {
        return  gameObject.GetComponent<IMakeMistakes>();
    }
}