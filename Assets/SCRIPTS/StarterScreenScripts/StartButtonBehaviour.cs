using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonBehaviour : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("SceneSelector");
    }
}
