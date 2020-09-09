using Assets.Scripts.BustAKlud;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KludCannon : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject barrel;
    public Animator smokeAnimator;
    public KludProvider kludProvider;
    public GameObject kludStart;
    public Transform kludContainer;
    public SpriteRenderer cannonSprite;

    public float kludSpeed = 20f;

    private KludControls controls;

    public void OnKludLoaded(GameObject newKlud)
    {
        if (newKlud.GetComponent<BustPiece>() is BustPiece piece)
        {
            this.cannonSprite.color = piece.color;
        }
    }

    private void Awake()
    {
        this.controls = new KludControls();
        this.controls.Bust.Fire.performed += ctx => this.Fire();
    }


    private void OnEnable()
    {
        this.controls.Enable();
    }

    private void OnDisable()
    {
        this.controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        var cursorPosition = this.controls.Bust.Point.ReadValue<Vector2>();
        var target = this.mainCamera.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, this.transform.position.z));
        var difference = target - this.transform.position;
        var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.barrel.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    private void Fire()
    {
        this.smokeAnimator.SetTrigger("Blast");

        var cursorPosition = this.controls.Bust.Point.ReadValue<Vector2>();
        var target = this.mainCamera.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, this.transform.position.z));
        var localTarget = this.kludContainer.InverseTransformPoint(target);
        localTarget += new Vector3(-.5f, .5f);
        target = this.kludContainer.TransformPoint(localTarget);
        var difference = target - this.transform.position;
        var distance = difference.magnitude;
        Vector3 direction = difference / distance;
        direction.Normalize();

        var klud = Instantiate(this.kludProvider.TakePrefab());
        klud.transform.parent = this.kludContainer;
        klud.transform.position = this.kludStart.transform.position;
        klud.transform.localScale = Vector3.one;
        klud.GetComponent<Rigidbody2D>().velocity = direction * this.kludSpeed;
        //Debug.Log($"direction ({direction}) * speed ({this.kludSpeed}) = {klud.GetComponent<Rigidbody2D>().velocity}");
    }
}
