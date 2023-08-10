using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorUnlock : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

}
