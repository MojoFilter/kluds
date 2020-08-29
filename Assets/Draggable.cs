using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{

    private KludControls controls;
    private bool isDragging;

    private void Awake()
    {
        controls = new KludControls();
        controls.Test.Drag.started += _ => isDragging = true;
        controls.Test.Drag.canceled += _ => isDragging = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void OnMouseDown()
    {
        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(controls.Test.Position.ReadValue<Vector2>()) - transform.position;
            transform.Translate(mousePosition + new Vector2(-.5f, .5f));
        }
    }
}
