using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingTestCube : InteractableObject, ISaveable
{
    [SerializeField] private int Clicks = 0;

    public override void Interact(InteractionController interactor)
    {
        Clicks++;
    }
    public object CaptureState()
    {
        Vector3 position = this.gameObject.transform.position;
        return new SaveData
        {
            x = position.x,
            y = position.y,
            z = position.z,
            clicks = Clicks
        };
    }

    public void RestoreState(object state)
    {
        SaveData saveData = (SaveData)state;

        this.gameObject.transform.position = new Vector3(saveData.x, saveData.y, saveData.z);
        this.Clicks = saveData.clicks;
    }

    [System.Serializable]
    private struct SaveData
    {
        public float x;
        public float y;
        public float z;
        public int clicks;
    }
}