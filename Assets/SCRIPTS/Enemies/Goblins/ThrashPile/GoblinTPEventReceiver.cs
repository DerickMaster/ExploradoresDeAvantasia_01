using UnityEngine;
using UnityEngine.Events;

public class GoblinTPEventReceiver : MonoBehaviour
{
    public UnityEvent shootEvent;
    public void ShootProjectile()
    {
        shootEvent.Invoke();
    }
}
