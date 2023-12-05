using UnityEngine;
using UnityEngine.Events;
using StageSelection;

public class CategoryInformation : MonoBehaviour
{
    [SerializeField] string CategoryName;
    [SerializeField] StageLoadInformation[] stagesInfo;

    [HideInInspector]public UnityEvent<CategoryInformation> categoryClicked;

    public StageLoadInformation[] GetStagesInfo()
    {
        return stagesInfo;
    }

    public void ButtonClicked()
    {
        categoryClicked.Invoke(this);
    }
}
