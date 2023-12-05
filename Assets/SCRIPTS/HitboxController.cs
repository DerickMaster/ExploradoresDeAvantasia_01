using System.Collections;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    private bool HitboxOpen = true;
    [SerializeField] private HealthManager hpManager;

    public float InvulnerabilityTime = 3f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Hazard") && HitboxOpen)
        {
            HitboxOpen = false;
            Debug.Log("dmg");
            hpManager.TakeDamage();
            StartCoroutine(InvulnerabilityPeriod());
        }
    }

    private IEnumerator InvulnerabilityPeriod()
    {
        float count = 0f;
        while (count < InvulnerabilityTime)
        {
            count += Time.deltaTime;
            yield return null;
        }
        HitboxOpen = true ;
    }
}
