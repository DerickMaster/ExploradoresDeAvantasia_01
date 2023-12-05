using UnityEngine;
using UnityEngine.UI;

public class MudPuddleBehaviour : MonoBehaviour
{
    [SerializeField] private float newMoveSPD;
    [SerializeField] private Material _mudMaterial;
    [SerializeField] private Sprite mudScreen;
    [SerializeField] private float timeToAppear;


    [TextArea] public string tempText;
    [SerializeField] float tempTextTime = 2f;
    [SerializeField] Sprite _forestSpiritSprite;
    [SerializeField] FMODUnity.EventReference _errorDub;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasBehaviour.Instance.SetActiveTempText(tempText, tempTextTime, _forestSpiritSprite, _errorDub);
            other.GetComponent<StarterAssets.ThirdPersonController>().MoveSpeed = newMoveSPD;
            ChangeCharacterMaterial(other.gameObject);
            try{ UIScreenEffect.Instance.SetScreenEffect(mudScreen, new Color(1, 1, 1, timeToAppear), 0.75f); }
            catch { Debug.Log("UIScreenEffect not avaible"); }
        }
    }

    private void ChangeCharacterMaterial(GameObject player)
    {
        player.GetComponentInChildren<ObjectMaterialController>().AddMaterial(_mudMaterial);
    }
}