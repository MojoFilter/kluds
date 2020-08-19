using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public AnimationCurve dropCurve;
    public float fallSpeed = 20.0f;
    public Vector2 fallDistance = new Vector2(0, -1);

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.targetPosition = this.transform.position;
        Debug.Log("Start position: " + this.targetPosition);
        this.StartCoroutine(this.FallOccasionally());
    }

    // Update is called once per frame
    void Update()
    {
        var step = this.fallSpeed * Time.deltaTime;
        step = this.dropCurve.Evaluate(step);
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPosition, step);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.rigidbody.velocity = Vector2.zero;
        collision.rigidbody.isKinematic = true;
    }

    private IEnumerator FallOccasionally()
    {
        while (true)
        {
            yield return new WaitForSeconds(4.0f);
            this.targetPosition = this.transform.position + (Vector3)fallDistance;
        }
    }

}
