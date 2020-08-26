using Assets.Scripts.BustAKlud;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public AnimationCurve dropCurve;
    public float fallSpeed = 20.0f;
    public Vector2 fallDistance = new Vector2(0, -1);
    public BustAKludController manager;

    private Vector3 targetPosition;


    // Start is called before the first frame update
    void Start()
    {
        this.targetPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var step = this.fallSpeed * Time.deltaTime;
        step = this.dropCurve.Evaluate(step);
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPosition, step);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        collision.rigidbody.velocity = Vector2.zero;
        collision.rigidbody.isKinematic = true;
        var snapOrigin = this.GetComponent<Collider2D>().bounds.ClosestPoint(collision.transform.position);
        
        //this.manager.Dock(snapOrigin, collision.gameObject);
    }

    public void Drop()
    {
        this.targetPosition = this.transform.position + (Vector3)fallDistance;
    }

}
