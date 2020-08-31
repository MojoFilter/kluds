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
        //public GameObject marker1Prefab;
        //public GameObject marker2Prefab;

        public LayerMask gridLayerMask;

        private bool _docked = false;
        private Rigidbody2D _body;

        private Vector2? _snap;

        private void Awake()
        {
            _body = this.GetComponent<Rigidbody2D>();
        }

        public void Pop()
        {
            this.GetComponent<Animator>().SetTrigger("Explode");
        }

        public void SetMooring(bool moored)
        {
            if (!moored)
            {
                _body.isKinematic = false;
                _body.gravityScale = 1f;
                _body.AddForce(new Vector2(Random.Range(-20f, 20f), Random.Range(10f, 500f)));
                this.transform.Find("Sectors").gameObject.SetActive(false);
                Destroy(this, 4f);
            }
        }

        //const float segmentLength = Mathf.PI * 2f / 6f;
        //string[] segmentNames = new[]
        //{
        //    "Upper-right",
        //    "Upper-left",
        //    "Left",
        //    "Lower-left",
        //    "Lower-Right",
        //    "Right"
        //};

        //Vector2[] segmentOffsets = new[]
        //{
        //    (-.5f, -1f),
        //    (.5f, -1f),
        //    (1f, 0f),
        //    (.5f, .5f),
        //    (-.5f, .5f),
        //    (-1f, 0f)
        //}.Select(i => new Vector2(i.Item1, i.Item2)).ToArray();

        private HashSet<string> _stoppers = new HashSet<string>() { "Crusher", "Klud" };

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_docked && _stoppers.Contains(collision.gameObject.tag))
            {
                this.SnapToGrid();
                FindObjectOfType<BustAKludController>().Dock(this.gameObject);
            }
        }

        //private Vector2 AnchorOf(Collision2D collision)
        //{
        //    if (collision.gameObject.CompareTag("Klud"))
        //    {
        //        return collision.transform.localPosition;
        //    }
        //    var hitPoint = this.transform.parent.InverseTransformPoint(collision.GetContact(0).point);
        //    Debug.LogWarning($"hitpoint {hitPoint}");
        //    var xroot = Mathf.Round(hitPoint.x + 0.5f) - 0.5f;
        //    return new Vector2(xroot, 0f);
        //}

        private void SnapToGrid()
        {
            var center = transform.Find("KludCenter");
            var snap = Physics2D.OverlapPoint(center.transform.position, this.gridLayerMask);
            if (snap != null)
            {
                _snap = snap.transform.position;
                _docked = true;
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
        }
    }
}
