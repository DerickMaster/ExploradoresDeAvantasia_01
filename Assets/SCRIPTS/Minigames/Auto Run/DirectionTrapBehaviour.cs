using UnityEngine;

public class DirectionTrapBehaviour : TrapBehaviour
{
    string[] directions = { "esquerda","direita" };

    [ContextMenu("Initialize")]
    public override void Initialize()
    {
        correctId = Random.Range(0, 2);

        CanvasBehaviour.Instance.SetActiveTempText("Pise no chao da " + directions[correctId]);
    }

    protected override bool CheckTile(TrapTileBehaviour tileStepped)
    {
        if (tileStepped.id == correctId)
            return true;
        else
        {
            message = "esse lado eh a " + directions[tileStepped.id];
            return false;
        }
    }
}
