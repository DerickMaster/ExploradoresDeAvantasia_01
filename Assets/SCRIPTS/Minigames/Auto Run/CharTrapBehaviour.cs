using UnityEngine;

public class CharTrapBehaviour : TrapBehaviour
{
    readonly string letters = "ZXCVBNMASDFGHJKLQWERTYUIOP";
    readonly string numbers = "123456879";
    bool isNumber;

    [ContextMenu("Initialize")]
    public override void Initialize()
    {
        isNumber = Random.value >= 0.5f;
        string randomChar;
        string questionMessage;
        if (isNumber)
        {
            questionMessage = "Passe no chao com o numero ";
            randomChar = numbers.Substring(Random.Range(0, numbers.Length), 1);
        }
        else
        {
            questionMessage = "Passe no chao com a letra ";
            randomChar = letters.Substring(Random.Range(0, letters.Length), 1);
        }

        CanvasBehaviour.Instance.SetActiveTempText(questionMessage + randomChar);

        correctId = Random.Range(0, m_Tiles.Length);

        foreach (var tile in m_Tiles)
        {
            if(tile.id == correctId)
                tile.SetText(randomChar);
            else if (isNumber)
                tile.SetText(letters.Substring(Random.Range(0, letters.Length), 1));
            else
                tile.SetText(numbers.Substring(Random.Range(0, numbers.Length), 1));
        }
    }

    protected override bool CheckTile(TrapTileBehaviour tileStepped)
    {
        if (tileStepped.id == m_Tiles[correctId].id)
            return true;
        else
        {
            if (isNumber)
                message = "voce pisou na letra ";
            else
                message = "voce pisou no numero ";
            message += tileStepped.GetText();

            return false;
        }
    }
}
