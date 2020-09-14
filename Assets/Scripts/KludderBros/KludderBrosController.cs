using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KludderBrosController : MonoBehaviour
{
    public List<Sprite> kludSprites = new List<Sprite>();
    public GameObject kludderPrefab;
    public Transform board;
    public Vector2 startForce;

    private KludControls _controls;

    private Rigidbody2D _launchBody;

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
        _controls.KludderBros.Start.performed += _ => StartGame();
    }


    private void StartGame()
    {
        var bigAssKludder = Instantiate(kludderPrefab, this.board);
        var sprite = kludSprites[Random.Range(0, kludSprites.Count)];
        bigAssKludder.GetComponent<SpriteRenderer>().sprite = sprite;
        bigAssKludder.GetComponent<Kludder>().startForce = startForce;
    }

    //private void FixedUpdate()
    //{
    //    if (_launchBody != null)
    //    {
    //        _launchBody.AddForce(new Vector2(0f, 20f), ForceMode2D.Impulse);
    //    }
    //}


}
