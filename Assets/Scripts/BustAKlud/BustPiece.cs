using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Assets.Scripts.BustAKlud
{
    public class BustPiece : MonoBehaviour
    {
        public SpriteRenderer kludRenderer;
        public Color color;
        public Sprite anchoredSprite;
        public Sprite unmooredSprite;

        public void Pop()
        {
            this.GetComponent<Animator>().SetTrigger("Explode");
        }

        public void SetMooring(bool moored)
        {
            this.kludRenderer.sprite = moored ? anchoredSprite : unmooredSprite;
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
            (.5f, 0f),
            (.5f, .5f),
            (-.5f, .5f),
            (-.5f, 0f)
        }.Select(i => new Vector2(i.Item1, i.Item2)).ToArray();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var body = this.GetComponent<Rigidbody2D>();
            if (body.velocity != Vector2.zero)
            {
                var stoppers = new[] { "Klud", "Crusher" };
                if (stoppers.Contains(collision.gameObject.tag))
                {
                    body.isKinematic = true;
                    var contactPoint = this.transform.InverseTransformPoint(collision.GetContact(0).point);
                    var relativeContactPoint = contactPoint - new Vector3(.5f, -.5f);
                    var hitRads = Mathf.Atan2(contactPoint.y, contactPoint.x);
                    var sector = Mathf.FloorToInt((hitRads + (segmentLength / 2f)) / segmentLength);
                    var hitAnchor = AnchorOf(collision);
                    var dockPoint = hitAnchor + segmentOffsets[sector];
                    Debug.LogError($"{hitAnchor} + {segmentOffsets[sector]}");
                    body.velocity = Vector2.zero;
                    this.transform.localPosition = dockPoint;
                    //Debug.LogError($"({hitRads} + ({segmentLength} / 2f)) / {segmentLength}");
                    //Debug.LogWarning($"{contactPoint} :: {relativeContactPoint} -- Sector #{sector} [{segmentNames[sector]}]");
                    //FindObjectOfType<BustAKludController>().Dock(this.gameObject, relativeContactPoint);
                }
            }
        }

        private Vector2 AnchorOf(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Klud"))
            {
                return collision.transform.localPosition;
            }
            var hitPoint = collision.transform.InverseTransformPoint(collision.GetContact(0).point);
            Debug.LogWarning($"hitpoint {hitPoint}");
            var xroot = Mathf.Round(hitPoint.x + 0.5f) - 0.5f;
            return new Vector2(xroot, 0f);
        }
    }
}
