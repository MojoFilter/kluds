using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BustAKlud
{
    public class BustBoard : MonoBehaviour
    {
        public int maxLines = 12;
        public int longRowWidth = 10;

        public Transform kludHolder;
        public LayerMask kludLayer;


        private void Awake()
        {
            
        }

        public void Dock(GameObject klud)
        {
            var kludBody = klud.GetComponent<Rigidbody2D>();
            kludBody.isKinematic = true;
            kludBody.velocity = Vector2.zero;
            var boardPosition = GetClampedPosition(klud.transform.localPosition, this);
            var xOffset = IsShortRow((int)boardPosition.y) ? 0.5f : 0f;
            klud.transform.localPosition = new Vector3(boardPosition.x + xOffset, boardPosition.y, 0);
        }

        public GameObject GetPiece(int line, int index)
        {
            Vector2 worldPoint = this.kludHolder.TransformPoint(line, index, 0);
            return Physics2D.OverlapPoint(worldPoint, this.kludLayer)?.gameObject;
        }

        public static Vector3 GetClampedPosition(Vector3 position, BustBoard board)
        {
            var y = Mathf.Clamp(Mathf.RoundToInt(position.y), -(board.maxLines - 1), 0);
            var shortLine = IsShortRow(y);
            var pos = Mathf.FloorToInt(position.x + (shortLine ? 0 : 0.5f));
            var x = Mathf.Clamp(pos, 0, shortLine ? 9 : 10);

            return new Vector3(x, y, 0);
        }

        public static Vector2Int GetLineAndPosition(Vector3 worldPosition, BustBoard board)
        {
            var pos = GetClampedPosition(worldPosition, board);
            return new Vector2Int(Mathf.Abs((int)pos.x), Mathf.Clamp(Mathf.Abs(Mathf.FloorToInt(pos.y)), 0, board.maxLines - 1));
        }

        private static bool IsShortRow(int rowIndex) => Math.Abs(rowIndex) % 2 == 1;
    }
}
