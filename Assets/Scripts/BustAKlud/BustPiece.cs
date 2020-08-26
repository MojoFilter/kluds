using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BustAKlud
{
    public class BustPiece : MonoBehaviour
    {
        public SpriteRenderer kludRenderer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var stoppers = new[] { "Klud", "Crusher" };
            if (stoppers.Contains(collision.gameObject.tag))
            {
                FindObjectOfType<BustAKludController>().Dock(this.gameObject);
            }
        }
    }
}
