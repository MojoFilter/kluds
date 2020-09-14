using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KludChainCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Roof"))
        {
        }
        Destroy(this.gameObject);
    }
}
