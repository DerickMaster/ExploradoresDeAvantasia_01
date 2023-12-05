using UnityEngine;
using System.Linq;

public class ColorTrapBehaviour : TrapBehaviour
{
    private struct ColorInfo
    {
        public string colorName;
        public Color color;
    }

    ColorInfo[] colors;

    private new void Awake()
    {
        base.Awake();

        colors = new ColorInfo[]
        {
            new ColorInfo{ colorName = "azul" , color = Color.blue},
            new ColorInfo{ colorName = "vermelho" , color = Color.red},
            new ColorInfo{ colorName = "amarelo" , color = Color.yellow},
            new ColorInfo{ colorName = "verde" , color = Color.green}
        };
    }

    [ContextMenu("Initialize")]
    public override void Initialize()
    {
        
        colors = colors.Shuffle().ToArray();

        correctId = Random.Range(0, m_Tiles.Length);
        int i = 0;
        foreach (var tile in m_Tiles)
        {
            tile.SetColor(colors[i].color);
            i++;
        }

        CanvasBehaviour.Instance.SetActiveTempText("Pise no chao de cor " + colors[correctId].colorName);
    }

    protected override bool CheckTile(TrapTileBehaviour tileStepped)
    {
        if (tileStepped.id == correctId)
            return true;
        else
        {
            message = "essa cor eh " + colors[tileStepped.id].colorName;
            return false;
        }
    }
}
