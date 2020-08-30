﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="klud"></param>
        /// <param name="contactPoint">
        ///   Relative to the klud center
        /// </param>
        public void Dock(GameObject klud)//, Vector2 contactPoint)
        {
            //var rads = Mathf.Atan2(contactPoint.y, contactPoint.x);
            //Debug.Log($"Hit at {rads * Mathf.Rad2Deg}° {contactPoint}");

            //var testPoints = new []
            //{
            //    (.5f, 0f),
            //    (.5f, .5f),
            //    (0f, .5f),
            //    (-.5f, .5f),
            //    (-.5f, 0f),
            //    (-.5f, -.5f),
            //    (0f, -.5f),
            //    (.5f, -.5f)
            //};
            //var result = string.Join(Environment.NewLine, testPoints.Select(p => $"({p.Item1}, {p.Item2}) => {Mathf.Atan2(p.Item2, p.Item1) * Mathf.Rad2Deg}"));
            //Debug.LogWarning(result);
            //var kludBody = klud.GetComponent<Rigidbody2D>();
            //kludBody.isKinematic = true;
            //kludBody.velocity = Vector2.zero;
            var boardPosition = GetClampedPosition(klud.transform.localPosition, this);
            //var xOffset = IsShortRow((int)boardPosition.y) ? 0.5f : 0f;
            //var dockedPosition = new Vector3(boardPosition.x + xOffset, boardPosition.y, 0);
            //Debug.LogError($"({klud.transform.localPosition.x}, {klud.transform.localPosition.y}) => [{boardPosition.x}, {boardPosition.y}] ({dockedPosition.x}, {dockedPosition.y})");
            //klud.transform.localPosition = dockedPosition;
            var lostKluds = this.PopMatches((int)boardPosition.y, (int)boardPosition.x);
            if (lostKluds.Any())
            {
                this.DropUnmoored(lostKluds);
            }
        }

        private IEnumerable<GameObject> PopMatches(int row, int column)
        {
            var matches = CollectMatches(row, column, this);
            //Debug.Log($"Matched {matches.Count()}");
            if (matches.Count() > 2)
            {
                foreach(var deadKlud in matches)
                {
                    deadKlud.transform.SetParent(this.transform);
                    deadKlud.GetComponent<BustPiece>().Pop();
                }
                return matches;
            }
            return Enumerable.Empty<GameObject>();
        }

        private void DropUnmoored(IEnumerable<GameObject> poppedKluds)
        {
            var anchored = CollectAnchored(this, poppedKluds);
            var allChildren = GetChildren(this.kludHolder.gameObject);
            var orphaned = allChildren.Where(c => !anchored.Contains(c));
            Debug.LogWarning($"Children: {allChildren.Count()}  Orphans: {orphaned.Count()}");
            foreach (var stable in anchored)
            {
                var klud = stable.GetComponent<BustPiece>();
                klud.SetMooring(true);
            }
            foreach (var orphan in orphaned)
            {
                var klud = orphan.GetComponent<BustPiece>();
                klud.SetMooring(false);
                //var body = orphan.GetComponent<Rigidbody2D>();
                //body.isKinematic = false;
                //body.gravityScale = 1f;
                //body.AddForce(new Vector2(Random.Range(-10f, 10f), Random.Range(10f, 100f)));
                //Destroy(orphan, 4f);
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

        //public static Vector2Int GetLineAndPosition(Vector3 worldPosition, BustBoard board)
        //{
        //    var pos = GetClampedPosition(worldPosition, board);
        //    return new Vector2Int(Mathf.Abs((int)pos.x), Mathf.Clamp(Mathf.Abs(Mathf.FloorToInt(pos.y)), 0, board.maxLines - 1));
        //}

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
                Collect(position, collected, new HashSet<Vector2Int>(), o => KludMatches(kludSprite, o), board);
            }
            return collected;
        }

        private static HashSet<GameObject> CollectAnchored(BustBoard board, IEnumerable<GameObject> poppedKluds)
        {
            var collected = new HashSet<GameObject>();
            var visited = new HashSet<Vector2Int>();
            for (int i = 0; i < board.longRowWidth; ++i)
            {
                Collect(new Vector2Int(i, 0), collected, visited, o => o != null && !poppedKluds.Contains(o), board);
            }
            return collected;
        }

        private static void Collect(Vector2Int position, HashSet<GameObject> collected, HashSet<Vector2Int> visited, Predicate<GameObject> pred, BustBoard board)
        {
            visited.Add(position);
            var thisKlud = GetPiece(position.y, position.x, board);
            if (pred(thisKlud))
            {
                collected.Add(thisKlud);
                var directions = IsShortRow(position.y) ? shortContactDirections : contactDirections;
                var newNeighbors = directions.Select(d => position + d)
                                             .Where(n => !visited.Contains(n))
                                             .ToArray();

                foreach (var n in newNeighbors)
                {
                    Collect(n, collected, visited, pred, board);
                }
            }
        }

        private static bool KludMatches(Sprite sprite, GameObject klud)
        {
            return klud?.GetComponent<BustPiece>()?.kludRenderer.sprite?.Equals(sprite) == true;
        }
    }
}
