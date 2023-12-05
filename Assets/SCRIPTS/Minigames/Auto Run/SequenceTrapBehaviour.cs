using UnityEngine;

public class SequenceTrapBehaviour : TrapBehaviour
{
    [ContextMenu("Initialize")]
    public override void Initialize()
    {
        correctId = Random.Range(0, 2);
        int sequence = 1;
        foreach (var tile in m_Tiles)
        {
            if (tile.id == correctId + ((sequence - 1) * 2))
            {
                tile.SetText(sequence.ToString());
                sequence++;
            }
            else tile.SetText("( ͡° ͜ʖ ͡°)");
        }
    }

    protected override bool CheckTile(TrapTileBehaviour tileStepped)
    {
        return (correctId % 2 == 0) == (tileStepped.id % 2 == 0);
    }
}