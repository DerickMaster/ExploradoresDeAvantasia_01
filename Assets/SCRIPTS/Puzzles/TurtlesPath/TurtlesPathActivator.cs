using UnityEngine;
using UnityEngine.Events;

public class TurtlesPathActivator : InteractableObject
{
    private TurtlesPathActivator[] activators;

    [SerializeField] private int[] turtlesIdOn;
    [SerializeField] private int[] turtlesIdOff;
    [SerializeField] Collider pathBlocker;
    [SerializeField] private bool selfDeactivated;

    private MaracaBehaviour maraca;
    private StarterAssets.ThirdPersonController player;
    private BoxCollider _deactivationZone;

    [HideInInspector] public UnityEvent<int, bool> activateTurtlesEvent;

    private new void Start()
    {
        base.Start();

        _deactivationZone = GetComponent<BoxCollider>();
        _deactivationZone.enabled = false;

        activators = transform.parent.GetComponentsInChildren<TurtlesPathActivator>();

        maraca = GetComponentInChildren<MaracaBehaviour>();
        maraca.Initialize();
        maraca.shakeFinishedEvent.AddListener(ActivateTurtles);
        maraca.gameObject.SetActive(false);
    }

    public override void Interact(InteractionController interactor)
    {
        if (!player) player = interactor.GetComponentInParent<StarterAssets.ThirdPersonController>();
        player.SwitchControl(true);
        maraca.gameObject.SetActive(true);
        maraca.Shake();
        if (singleInteraction) DeactivateInteractable();
    }

    private void ActivateTurtles()
    {
        maraca.gameObject.SetActive(false);
        pathBlocker.enabled = false;

        foreach (int id in turtlesIdOn)
        {
            activateTurtlesEvent.Invoke(id, true);
        }
        
        foreach (int id in turtlesIdOff)
        {
            activateTurtlesEvent.Invoke(id, false);
        }

        if(player) player.SwitchControl(false);

        if (!selfDeactivated) return;

        foreach (TurtlesPathActivator activator in activators)
        {
            if(activator != this)
            {
                activator.SetActiveDeactivationZone(true);
                break;
            }
        }
    }

    public void SetActiveDeactivationZone(bool active)
    {
        pathBlocker.enabled = !active;
        _deactivationZone.enabled = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (int id in turtlesIdOn)
            {
                activateTurtlesEvent.Invoke(id, false);
            }
            SetActiveDeactivationZone(false);
        }
    }
}
