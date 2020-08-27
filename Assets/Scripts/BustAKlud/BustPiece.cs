﻿using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BustAKlud
{
    public class BustPiece : MonoBehaviour
    {
        public SpriteRenderer kludRenderer;

        public void Pop()
        {
            this.GetComponent<Animator>().SetTrigger("Explode");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (this.GetComponent<Rigidbody2D>()?.velocity != Vector2.zero)
            {
                var stoppers = new[] { "Klud", "Crusher" };
                if (stoppers.Contains(collision.gameObject.tag))
                {
                    FindObjectOfType<BustAKludController>().Dock(this.gameObject);
                }
            }
        }
    }
}
