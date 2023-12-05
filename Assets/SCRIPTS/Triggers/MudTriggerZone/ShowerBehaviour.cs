using UnityEngine;

public class ShowerBehaviour : MonoBehaviour
{
    public int SpeedValue = 8;
    public Animator showerRobot;
    public GameObject particle;
    public GameObject WateryFloor;
    public StarterAssets.ThirdPersonController player;
    private bool playerBelow = false;

    [SerializeField] private string materialName;
    [SerializeField] private float screenCleanDuration;

    public void Shower()
    {
        if (playerBelow) CleanUp();
    }

    private void CleanUp()
    {
        player.gameObject.GetComponentInChildren<ObjectMaterialController>().RemoveMaterial(materialName);
        player.MoveSpeed = SpeedValue;
        try { UIScreenEffect.Instance.DeactivateScreenEffect(screenCleanDuration); }
        catch { Debug.Log("UIScreenEffect not avaible"); }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<StarterAssets.ThirdPersonController>();
            showerRobot.SetBool("Active", true);
            playerBelow = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerBelow = false;
            showerRobot.SetBool("Active", false);
        }
    }

    public void ShowerParticle(int state)
    {
        if(state == 1)
        {
            Shower();
            particle.SetActive(true);
            WateryFloor.SetActive(true);
        }
        if (state == 0)
        {
            particle.SetActive(false);
            WateryFloor.SetActive(false);
        }
    }
}
