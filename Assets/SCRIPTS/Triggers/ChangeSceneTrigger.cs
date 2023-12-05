using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeSceneTrigger : TriggerZone
{
    public string _sceneName;
    public override void Triggered(Collider other)
    {
        SceneManager.LoadScene(_sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
