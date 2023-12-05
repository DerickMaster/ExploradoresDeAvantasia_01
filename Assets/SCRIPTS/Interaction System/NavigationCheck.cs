using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationCheck : EventTrigger
{
    private InteractButtonBehaviour buttonBehaviour;

    public void Initialize(InteractButtonBehaviour buttonBehaviour)
    {
        this.buttonBehaviour = buttonBehaviour;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        buttonBehaviour.SetImage(true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        buttonBehaviour.SetImage(false);
    }
}