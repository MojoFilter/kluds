using System.Linq;
using UnityEngine;

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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var body = this.GetComponent<Rigidbody2D>();
            if (body.velocity != Vector2.zero)
            {
                var stoppers = new[] { "Klud", "Crusher" };
                if (stoppers.Contains(collision.gameObject.tag))
                {
                    body.isKinematic = true;
                    FindObjectOfType<BustAKludController>().Dock(this.gameObject);
                }
            }
        }
    }
}
