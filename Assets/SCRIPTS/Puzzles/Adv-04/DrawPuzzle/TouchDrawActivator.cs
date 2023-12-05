using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDrawActivator : InteractableObject
{
    TouchDraw draw;
    [SerializeField] Camera m_camera;
    [SerializeField] Canvas m_Canvas;

    private new void Start()
    {
        draw = GetComponent<TouchDraw>();
        draw.drawingSubmited.AddListener(PuzzleFinished);
    }
    public override void Interact(InteractionController interactor)
    {
        mainCameraRef = Camera.main;
        playerInteractor = interactor;
        SetActiveDrawing(true);
        draw.Initialize(interactor, m_camera);
    }
    Camera mainCameraRef;
    InteractionController playerInteractor;
    private void SetActiveDrawing(bool active)
    {
        draw.enabled = active;
        m_Canvas.gameObject.SetActive(active);
        m_camera.gameObject.SetActive(active);
        mainCameraRef.enabled = !active;
        CanvasBehaviour.Instance.SetActivePlayerUI(!active);
        CanvasBehaviour.Instance.SetActiveButtonList(!active);
        if (active) playerInteractor.SwitchActionMap("Drawing");
        else playerInteractor.SwitchActionMap();
    }

    private void PuzzleFinished()
    {
        SetActiveDrawing(false);
    }

    public void ResetLines()
    {
        draw.ClearLines();
    }
}
