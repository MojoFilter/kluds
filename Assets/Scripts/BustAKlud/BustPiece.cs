using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Assets.Scripts.BustAKlud
{
    public class BustPiece : MonoBehaviour
    {
        public SpriteRenderer kludRenderer;
        public Color color;
        public LayerMask gridLayerMask;

        private bool _docked = false;
        private Rigidbody2D _body;

        private Vector2? _snap;
        private bool _unmoor;

        private void Awake()
        {
            _body = this.GetComponent<Rigidbody2D>();
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
            _docked = true;
            var center = transform.Find("KludCenter");
            var snap = Physics2D.OverlapPoint(center.transform.position, this.gridLayerMask);
            if (snap != null)
            {
                _snap = snap.transform.position;
            }
        }

        private void FixedUpdate()
        {
            if (_snap.HasValue)
            {
                _body.velocity = Vector2.zero;
                _body.isKinematic = true;
                _body.position = _snap.Value;
                _snap = null;
            }
            if (_unmoor)
            {
                _unmoor = false;
                _body.isKinematic = false;
                _body.gravityScale = 1f;
                _body.AddForce(new Vector2(Random.Range(-20f, 20f), Random.Range(10f, 500f)));
            }
        }
    }
}
