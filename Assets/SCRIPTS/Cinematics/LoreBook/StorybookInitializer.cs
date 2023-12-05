using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorybookInitializer : MonoBehaviour
{
    public LoreBookBehaviour _loreBook;

    private void Start()
    {
        _loreBook.InitializeBook();
    }
}
