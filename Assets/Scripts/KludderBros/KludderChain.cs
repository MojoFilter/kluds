using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class KludderChain : MonoBehaviour
{
    public UnityEvent chainDestroyed = new UnityEvent();

    public float speed = 4f;

    private float _length = 0;

    private void Update()
    {
        _length += this.speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        this.transform.localScale = new Vector3(1f, _length);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Klud"))
        {
            collision.gameObject.GetComponent<Kludder>().Split();
        }
        this.chainDestroyed.Invoke();
        Destroy(this.gameObject);
    }

}

