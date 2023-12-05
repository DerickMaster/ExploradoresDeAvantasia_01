using UnityEngine;
using UnityEngine.UI;

public class SelectSceneButton : MonoBehaviour
{
    public int id;
    public string sceneName;
    public bool has_Intro;
    public bool started;

    public void Initialize(SceneInformation sceneInfo, int id)
    {
        this.id = id;
        sceneName = sceneInfo.sceneName;
        has_Intro = sceneInfo.has_Intro;
        started = sceneInfo.started;

        gameObject.GetComponentInChildren<Text>().text = sceneName;
    }

    public void ButtonClicked()
    {
        SceneSelectionManager.Instance.ButtonClicked(this);
    }
}
