using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KludderChain : MonoBehaviour
{
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
}

