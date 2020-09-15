using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HarryController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D body;

    public Transform board;
    public GameObject kludderChainPrefab;

    public float speed;

    private KludControls _controls;

    private float _velocity = 0f;
    private bool _isFiring;

    public void OnChainDestroyed()
    {
        _isFiring = false;
    }


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
        if (!_isFiring)
        {
            var chain = Instantiate(kludderChainPrefab);
            chain.transform.SetParent(this.board);
            chain.transform.position = new Vector3(this.transform.position.x, chain.transform.position.y, 0f);
            //chain.transform.localScale = new Vector3(1f, 0f, 1f);
            chain.GetComponent<KludderChain>().chainDestroyed.AddListener(new UnityAction(this.OnChainDestroyed));
            _isFiring = true;
        }
    }
}
