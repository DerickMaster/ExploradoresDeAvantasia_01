using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonClickImgChng : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Image _myImage;
    [SerializeField] Sprite _normalSprite, _pressedSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        _myImage.sprite = _pressedSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _myImage.sprite = _normalSprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        _myImage = GetComponent<Image>();
        _normalSprite = _myImage.sprite;
    }
}
