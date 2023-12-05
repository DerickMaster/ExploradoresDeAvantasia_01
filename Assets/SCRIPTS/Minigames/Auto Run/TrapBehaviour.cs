using UnityEngine;
using UnityEngine.Events;

public class TrapBehaviour : MonoBehaviour
{
    protected int correctId;
    protected TrapTileBehaviour[] m_Tiles;
    protected string message;
    [HideInInspector] public UnityEvent<bool, string> trapTriggered;

    protected void Awake()
    {
        m_Tiles = GetComponentsInChildren<TrapTileBehaviour>();

        int i = 0;
        foreach (var tile in m_Tiles)
        {
            tile.id = i;
            tile.tileStepped.AddListener(TileStepped);
            i++;
        }
    }

    protected void TileStepped(TrapTileBehaviour tileStepped)
    {
        bool correct = CheckTile(tileStepped);
        trapTriggered.Invoke(correct, "Placeholder");

       // if (message.Length == 0) return;

        if (correct) CanvasBehaviour.Instance.SetActiveTempText("Correto");
        else CanvasBehaviour.Instance.SetActiveTempText("incorreto " + message);
    }

    protected virtual bool CheckTile(TrapTileBehaviour tileStepped) { return false; }
    public virtual void Initialize() { }
}