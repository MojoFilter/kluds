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
            this.PopMatches((int)boardPosition.y, (int)boardPosition.x);
        }

        private void PopMatches(int row, int column)
        {
            var matches = CollectMatches(row, column, this);
            Debug.Log($"Matched {matches.Count()}");
            if (matches.Count() > 2)
            {
                foreach(var deadKlud in matches)
                {
                    deadKlud.GetComponent<BustPiece>().Pop();
                }
            }
        }


        private static GameObject GetPiece(int line, int index, BustBoard board)
        {
            float indexPosition = index + (IsShortRow(line) ? .5f : 0f);
            return GetChildren(board.kludHolder.gameObject)
                     .FirstOrDefault(k => k.transform.localPosition.y == line && k.transform.localPosition.x == indexPosition);
                     
            //Vector2 worldPoint = board.kludHolder.TransformPoint(line, indexPosition, 0);
            //Debug.Log($"Testing position ({index}, {line}) => point ({worldPoint.x}, {worldPoint.y}");
            //return Physics2D.OverlapPoint(worldPoint, board.kludLayer)?.gameObject;
            
        }

        static IEnumerable<GameObject> GetChildren(GameObject obj)
        {
            for (int i = 0; i < obj.transform.childCount; ++i)
            {
                yield return obj.transform.GetChild(i).gameObject;
            }
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

        private static readonly Vector2Int[] contactDirections = new[]
        {
            (-1, 1),
            (0, 1),
            (-1, 0),
            (1, 0),
            (-1, -1),
            (0, -1)
        }.Select(t => new Vector2Int(t.Item1, t.Item2)).ToArray();

        private static readonly Vector2Int[] shortContactDirections = new[]
        {
            (-1, 1),
            (0, 1),
            (-1, 0),
            (1, 0),
            (-1, -1),
            (0, -1)
        }.Select(t => new Vector2Int(t.Item1, t.Item2)).ToArray();

        private static IEnumerable<GameObject> CollectMatches(int row, int col, BustBoard board)
        {
            var startKlud = GetPiece(row, col, board);
            if (startKlud is null)
            {
                Debug.LogError($"Start klud not found at ({row}, {col})");
            }

            var collected = new HashSet<GameObject>();
            if (startKlud.GetComponent<BustPiece>() is BustPiece piece)
            {
                var kludSprite = piece.kludRenderer.sprite;
                var position = new Vector2Int(col, row);
                CollectMatches(kludSprite, position, collected, new HashSet<Vector2Int>(), board);
            }
            return collected;
        }

        private static void CollectMatches(Sprite match, Vector2Int position, HashSet<GameObject> collected, HashSet<Vector2Int> visited, BustBoard board)
        {
            visited.Add(position);
            var thisKlud = GetPiece(position.y, position.x, board);
            if (KludMatches(match, thisKlud))
            {
                collected.Add(thisKlud);
                var directions = IsShortRow(position.y) ? shortContactDirections : contactDirections;
                var newNeighbors = directions.Select(d => position + d)
                                             .Where(n => !visited.Contains(n))
                                             .ToArray();

                foreach (var n in newNeighbors)
                {
                    CollectMatches(match, n, collected, visited, board);
                }
            }
        }

        private static bool KludMatches(Sprite sprite, GameObject klud)
        {
            return klud?.GetComponent<BustPiece>()?.kludRenderer.sprite?.Equals(sprite) == true;
        }
    }
}
