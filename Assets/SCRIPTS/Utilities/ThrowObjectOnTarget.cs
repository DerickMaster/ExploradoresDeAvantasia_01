using System.Collections;
using UnityEngine;

public class ThrowObjectOnTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject targetLocation;
    [SerializeField]
    private GameObject _char;

    private void Start()
    {
        _char = FindObjectOfType<CharacterManager>().GetCurrentCharacter();
    }

    [ContextMenu("MoveObject")]
    public void MoveObject()
    {
        _char.GetComponent<StarterAssets.ThirdPersonController>().TakeKnockback(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _char = other.gameObject;
            MoveObject();
        }
            
    }
}
