using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousingingit : MonoBehaviour
{

    public bool lockCursor = true;

    private void Start()
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }
    void Update()
    {

       
    }
}