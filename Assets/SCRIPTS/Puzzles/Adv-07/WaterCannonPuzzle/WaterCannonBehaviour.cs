using UnityEngine;
public class WaterCannonBehaviour : InteractableObject
{
    private int shotValue = 0;
    private Animator _animator;
    private NumberSignBehaviour numberSign;

    private new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();

        foreach (WaterPumpBehaviour pumpBehaviour in transform.parent.GetComponentsInChildren<WaterPumpBehaviour>())
        {
            pumpBehaviour.pumpPressed.AddListener(ModifyShotValue);
        }
        numberSign = transform.parent.GetComponentInChildren<NumberSignBehaviour>();
        numberSign.TriggerChange(shotValue);
    }

    public override void Interact(InteractionController interactor)
    {
        Shoot();
    }

    private void Shoot()
    {
        DeactivateInteractable();
        _animator.SetTrigger("Activated");
    }

    public void Reactivate()
    {
        ReactivateInteractable();
    }

    private void ModifyShotValue(int mod)
    {
        shotValue += mod;
        if (shotValue < 0) shotValue = 0;
        else if(shotValue > 9) shotValue = 9;
        numberSign.TriggerChange(shotValue);
        _animator.SetFloat("Force", shotValue+1);
    }
}
