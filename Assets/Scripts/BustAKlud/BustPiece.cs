using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BustAKlud
{
    public class BustPiece : MonoBehaviour
    {
        public SpriteRenderer kludRenderer;
        public Color color;
        public LayerMask gridLayerMask;

        private bool _docked = false;
        private Rigidbody2D _body;
        private Transform _kludCenter;

        private Vector2? _snap;
        private bool _shouldSnap;
        private bool _unmoor;

        private void Awake()
        {
            _body = this.GetComponent<Rigidbody2D>();
            _kludCenter = this.transform.Find("KludCenter");
        }

        public void Pop()
        {
            this.GetComponent<Animator>().SetTrigger("Explode");
            //this.kludRenderer.color = Color.magenta;
        }

        public void SetMooring(bool moored)
        {
            if (!moored)
            {
                //this.kludRenderer.color = new Color(0xff, 0xff, 0xff, 0x88);
                _unmoor = true;
                this.GetComponent<Collider2D>().enabled = false;
                Destroy(this, 4f);
            }
        }

        private HashSet<string> _stoppers = new HashSet<string>() { "Crusher", "Klud" };

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_docked && _stoppers.Contains(collision.gameObject.tag))
            {
                this.SnapToGrid();
                FindObjectOfType<BustAKludController>().Dock(this.gameObject);
            }
        }


        public void SnapToGrid()
        {
            _shouldSnap = true;
            _docked = true;
            
        }

        private void FixedUpdate()
        {
            if (_shouldSnap)
            {
                var snap = Physics2D.OverlapPoint(_kludCenter.position, this.gridLayerMask);
                if (snap != null)
                {
                    _shouldSnap = false;
                    _body.velocity = Vector2.zero;
                    _body.isKinematic = true;
                    _body.position = snap.transform.position;
                }
            }
            if (_unmoor)
            {
                _unmoor = false;
                _body.isKinematic = false;
                _body.gravityScale = Random.Range(0.75f, 1.75f);
                //_body.AddForce(new Vector2(Random.Range(-20f, 20f), Random.Range(10f, 500f)));
            }
        }
    }
}
