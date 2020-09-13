using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KludderBrosController : MonoBehaviour
{
    private KludControls _controls;

    private void Awake()
    {
        _controls = new KludControls();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

}
