using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BustAKlud
{
    public class BustBoard : MonoBehaviour
    {
        public int maxLines = 12;

        public List<BustLine> lines = new List<BustLine>();

        private void Awake()
        {
            lines.Clear();
            lines.AddRange(Enumerable.Range(0, maxLines).Select(i => new BustLine(i % 2 == 1)));
        }

        public void Dock(Vector3 snapOrigin, GameObject klud)
        {
            //var kludTransform = klud.GetComponent<Transform>();
            //var pos = kludTransform.localPosition;
            //var left = pos.x;

            //var snapPosition = snapOrigin + (klud.transform.localPosition - snapOrigin).normalized;
            var linePos = GetLineAndPosition(snapOrigin, this);
            Debug.Log($"Should be snapping to {linePos}");
            var finalPosition = new Vector3(linePos.x + (this.lines[linePos.y].IsShortLine ? 0.5f : 0), -linePos.y);
            var otherPiece = this.GetPiece(linePos.y, linePos.x);
            var otherPiecePosition = otherPiece?.transform.localPosition;
        }

        public BustPiece GetPiece(int line, int index)
        {
            return this.lines.ElementAtOrDefault(line)?.ElementAtOrDefault(index);
        }

        public static Vector3 GetClampedPosition(Vector3 position, BustBoard board)
        {
            var y = Mathf.Clamp(Mathf.RoundToInt(position.y), -(board.maxLines - 1), 0);
            var shortLine = board.lines[Mathf.Abs(y)].IsShortLine;
            var pos = Mathf.FloorToInt(position.x + (shortLine ? 0 : 0.5f));
            var x = Mathf.Clamp(pos, 0, shortLine ? 6 : 7);

            return new Vector3(x, y, 0);
        }

        public static Vector2Int GetLineAndPosition(Vector3 worldPosition, BustBoard board)
        {
            var pos = GetClampedPosition(worldPosition, board);
            return new Vector2Int(Mathf.Abs((int)pos.x), Mathf.Clamp(Mathf.Abs(Mathf.FloorToInt(pos.y)), 0, board.maxLines - 1));
        }
    }
}
