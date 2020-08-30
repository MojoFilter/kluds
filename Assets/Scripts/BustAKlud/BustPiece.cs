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
        public GameObject marker1Prefab;
        public GameObject marker2Prefab;

        public LayerMask gridLayerMask;

        private bool _docked = false;
        private Rigidbody2D _body;

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

        const float segmentLength = Mathf.PI * 2f / 6f;
        string[] segmentNames = new[]
        {
            "Upper-right",
            "Upper-left",
            "Left",
            "Lower-left",
            "Lower-Right",
            "Right"
        };

        Vector2[] segmentOffsets = new[]
        {
            (-.5f, -1f),
            (.5f, -1f),
            (1f, 0f),
            (.5f, .5f),
            (-.5f, .5f),
            (-1f, 0f)
        }.Select(i => new Vector2(i.Item1, i.Item2)).ToArray();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_docked)
            {
                if (collision.gameObject.CompareTag("Crusher"))
                {
                    this.SnapToGrid();
                    //_docked = true;
                    //var x = Mathf.Round(this.transform.localPosition.x);
                    //var body = this.GetComponent<Rigidbody2D>();
                    //body.velocity = Vector2.zero;
                    //body.isKinematic = true;
                    //this.transform.localPosition = new Vector3(x, 0f);
                }
                else if (collision.gameObject.CompareTag("Klud")
                         && collision.otherCollider.gameObject.GetComponent<KludSector>() is KludSector sector)
                {
                    Debug.Log($"Hit {collision.gameObject.name} on {sector.name}");
                    _docked = true;
                    var body = this.GetComponent<Rigidbody2D>();
                    body.velocity = Vector2.zero;
                    body.isKinematic = true;
                    var otherPos = collision.transform.localPosition;
                    this.transform.localPosition = otherPos + sector.offset;
                    Debug.Log($"Positioning on {otherPos} by {sector.offset} => {transform.localPosition}");
                    //body.isKinematic = true;
                    //body.velocity = Vector2.zero;
                    //var y = Mathf.Round(this.transform.localPosition.y);
                    //var xOffset = ((int)Mathf.Abs(y) % 2 == 1) ? 0f : .5f;
                    //var x = Mathf.Round(this.transform.localPosition.x + xOffset) - xOffset;
                    //this.transform.localPosition = new Vector3(x, y);

                    //var contacts = new List<ContactPoint2D>();
                    //collision.GetContacts(contacts);
                    //var contactWorldPoint = contacts.Select(c => c.point).OrderBy(p => p.y).First();
                    //var contactPoint = this.transform.InverseTransformPoint(contactWorldPoint);
                    ////var relativeContactPoint = contactPoint - new Vector3(.5f, -.5f);
                    //var hitRads = Mathf.Atan2(contactPoint.y, contactPoint.x);
                    //var sector = Mathf.FloorToInt((hitRads + (segmentLength / 2f)) / segmentLength);
                    //Debug.LogWarning($"Hit in sector {sector} from {hitRads}");
                    //Vector3 dock = collision.transform.localPosition + (Vector3)segmentOffsets[sector];
                    //this.transform.localPosition = dock;
                    //Debug.Log($"Hit {collision.gameObject.name} at ({collision.transform.localPosition}) and docked at {transform.localPosition}");
                    //var hitAnchor = AnchorOf(collision);
                    //var dockPoint = hitAnchor + segmentOffsets[sector];
                    //Debug.LogError($"{hitAnchor} + {segmentOffsets[sector]}");
                    //body.velocity = Vector2.zero;
                    //this.transform.localPosition = dockPoint;
                    //Debug.LogError($"({hitRads} + ({segmentLength} / 2f)) / {segmentLength}");
                    //Debug.LogWarning($"{contactPoint} :: {relativeContactPoint} -- Sector #{sector} [{segmentNames[sector]}]");
                    FindObjectOfType<BustAKludController>().Dock(this.gameObject);
                }
            }
        }

        private Vector2 AnchorOf(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Klud"))
            {
                return collision.transform.localPosition;
            }
            var hitPoint = this.transform.parent.InverseTransformPoint(collision.GetContact(0).point);
            Debug.LogWarning($"hitpoint {hitPoint}");
            var xroot = Mathf.Round(hitPoint.x + 0.5f) - 0.5f;
            return new Vector2(xroot, 0f);
        }

        private void SnapToGrid()
        {
            var center = transform.Find("KludCenter");
            //var snapOrigin = this.transform.localPosition;// + new Vector3(.5f, -.5f);
            //var worldOrigin = this.transform.TransformPoint(snapOrigin);
            //Debug.Log($"Local {snapOrigin} World {worldOrigin} [{this.transform.localPosition}]");
            //var marker = Instantiate(marker1Prefab);
            //marker.transform.position = worldOrigin;

            //marker = Instantiate(marker2Prefab);
            //marker.transform.position = center.transform.position;

            var snap = Physics2D.OverlapPoint(center.transform.position, this.gridLayerMask);
            if (snap != null)
            {
                Debug.LogError($"Got a {snap.name}");
                _body.velocity = Vector2.zero;
                _body.isKinematic = true;
                _docked = true;
            }
            else
            {
                Debug.LogError("Didn't find shit");
            }
        }
    }
}
