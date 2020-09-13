using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarryController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D body;

    public GameObject kludderChainPrefab;

    public float speed;

    private KludControls _controls;

    private float _velocity = 0f;

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

    private void Start()
    {
        _controls.KludderBros.Fire.performed += _ => this.Fire();
    }

    private void Update()
    {
        var axis = _controls.KludderBros.HarryX.ReadValue<float>();
        _velocity = axis * speed;
        this.animator.SetFloat("Speed", _velocity);
    }

    private void FixedUpdate()
    {
        this.body.velocity = new Vector2(_velocity, 0f);
    }

    private void Fire()
    {
        var chain = Instantiate(kludderChainPrefab);
        chain.transform.position = new Vector3(this.transform.position.x, chain.transform.position.y, 0f);

    }
}
