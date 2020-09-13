using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
//using Random = UnityEngine.Random;

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
            var boardPosition = RoundPosition(klud.transform.localPosition);
            var lostKluds = this.PopMatches(boardPosition.x, boardPosition.y);
            if (lostKluds.Any())
            {
                this.DropUnmoored(lostKluds);
            }
        }

        public IEnumerable<Color> GetKludTypesInPlay()
        {
            return GetChildren(this.kludHolder.gameObject)
                .Select(g => g.GetComponent<BustPiece>())
                .Where(k => k is BustPiece)
                .Select(k => k.color);
        }

        public static (float x, float y) RoundPosition(Vector3 pos)
        {
            //float roundHalf(float v) => Mathf.Round(v * 2f) / 2f;
            var row = Mathf.Floor(pos.y);
            var isShifted = IsShortRow((int)row);
            float offset = isShifted ? .5f : 0f;
            var col = Mathf.Floor(pos.x - offset) + offset;
            var floored = new Vector2(col, row);
            var rounded = contactDirections
                .Concat(new[] { new Vector2(0f, 0f) })
                .Select(dir => floored + dir)
                .OrderBy(place => Vector2.Distance(pos, place))
                .Select(p => (p.x, p.y))
                .First();
            
            //var rounded = (roundHalf(pos.x), roundHalf(pos.y));
            Debug.Log($"Rounded {pos} to {rounded}");
            return rounded;
        }

        private IEnumerable<GameObject> PopMatches(float x, float y)
        {
            var matches = CollectMatches(x, y, this);
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
            var allChildren = GetChildren(this.kludHolder.gameObject).Select(p => p.gameObject);
            var orphaned = allChildren.Where(c => !anchored.Contains(c));
            //Debug.LogWarning($"Children: {allChildren.Count()}  Orphans: {orphaned.Count()}");
            foreach (var stable in anchored)
            {
                var klud = stable.GetComponent<BustPiece>();
                klud.SetMooring(true);
            }
            foreach (var orphan in orphaned)
            {
                if (orphan.TryGetComponent(out BustPiece klud))
                {
                    klud.SetMooring(false);
                }
                else
                {
                    var component = string.Join(" / ", orphan.GetComponents<Component>().Select(c => c.GetType().Name));
                    Debug.LogError($"Somehow got a {orphan.name} in the orphans ... {component}");
                }
                //var body = orphan.GetComponent<Rigidbody2D>();
                //body.isKinematic = false;
                //body.gravityScale = 1f;
                //Destroy(orphan, 4f);
            }
        }

        public void PlaceKlud(int row, int col, GameObject kludPrefab)
        {
            var klud = Instantiate(kludPrefab, this.kludHolder);
            var rowOffset = IsShortRow(row) ? 0.5f : 0f;
            klud.transform.localPosition = new Vector3(col + rowOffset, (float)-row);
            klud.GetComponent<Rigidbody2D>().isKinematic = true;
            klud.GetComponent<BustPiece>().SnapToGrid();
        }

        private static GameObject GetPiece(float x, float y, BustBoard board)
        {
            (x, y) = RoundPosition(new Vector3(x, y));
            var pointA = board.kludHolder.transform.TransformPoint(new Vector2(x + .25f, y - .25f));
            var pointB = board.kludHolder.transform.TransformPoint(new Vector2(x + .75f, y - .75f));
            return Physics2D.OverlapArea(pointA, pointB, board.kludLayer)?.gameObject;
        }

        static IEnumerable<BustPiece> GetChildren(GameObject obj)
        {
            return obj.transform.GetComponentsInChildren<BustPiece>();
            //for (int i = 0; i < obj.transform.childCount; ++i)
            //{
            //    yield return obj.transform.GetChild(i).gameObject;
            //}
        }


        private static bool IsShortRow(int rowIndex) => Math.Abs(rowIndex) % 2 == 1;

        private static readonly Vector2[] contactDirections = new[]
        {
            (1f, 0f),  //right
            (.5f, 1f), // top-right
            (-.5f, 1f),// top-left
            (-1f, 0f), // left
            (-.5f, -1f),// bottom-left
            (.5f, -1f) // bottom-right
        }.Select(t => new Vector2(t.Item1, t.Item2)).ToArray();


        private static IEnumerable<GameObject> CollectMatches(float x, float y, BustBoard board)
        {
            var startKlud = GetPiece(x, y, board);
            if (startKlud is null)
            {
                Debug.LogError($"Start klud not found at ({x}, {y})");
            }

            var log = new XDocument(new XElement("Matching"));
            var collected = new HashSet<GameObject>();
            if (startKlud.GetComponent<BustPiece>() is BustPiece piece)
            {
                var kludColor = piece.color;
                var position = new Vector2(x, y);
                Collect(position, collected, new HashSet<Vector2>(), o => KludMatches(kludColor, o), board, log.Root, Color.red);
            }
            //Debug.Log(log.ToString());
            return collected;
        }

        private static HashSet<GameObject> CollectAnchored(BustBoard board, IEnumerable<GameObject> poppedKluds)
        {
            var collected = new HashSet<GameObject>();
            var visited = new HashSet<Vector2>();
            for (int i = 0; i < board.longRowWidth; ++i)
            {
                Collect(new Vector2(i, 0), collected, visited, o => o != null && !poppedKluds.Contains(o), board, new XElement("root"), Color.green);
            }
            return collected;
        }

        private static void Collect(
            Vector2 position,
            HashSet<GameObject> collected,
            HashSet<Vector2> visited,
            Predicate<GameObject> pred,
            BustBoard board,
            XElement logParent,
            Color debugColor)
        {
            visited.Add(position);
            //var thisKlud = GetPiece(position.x, position.y, board);
            if (GetPiece(position.x, position.y, board) is GameObject thisKlud && pred(thisKlud))
            {
                var logNode = new XElement(XmlConvert.EncodeName(thisKlud.name), 
                    new XAttribute("id", thisKlud.GetInstanceID()),
                    new XAttribute("pos", $"{RoundPosition(thisKlud.transform.localPosition)}"),
                    new XAttribute("from", position),
                    new XAttribute("actual", thisKlud.transform.localPosition));

                logParent.Add(logNode);
                collected.Add(thisKlud);
                var newNeighbors = contactDirections
                    .Select(d => position + d)
                    .Where(n => !visited.Contains(n))
                    .ToArray();

                foreach (var n in newNeighbors)
                {
                    var centerOffset = new Vector2(.5f, -.5f);
                    var worldPosition = board.kludHolder.TransformPoint(position + centerOffset);
                    var worldDir = board.kludHolder.TransformPoint(n + centerOffset) - worldPosition;

                    var matchNode = new XElement("match", new XAttribute("n", n));
                    logNode.Add(matchNode);

                    Debug.DrawRay(worldPosition, worldDir, debugColor, 20f);
                    Collect(n, collected, visited, pred, board, matchNode, debugColor);
                }
            }
        }

        private static bool KludMatches(Color kludColor, GameObject klud)
        {            
            var matches = klud?.GetComponent<BustPiece>()?.color == kludColor;
            //if (klud != null)
            //{
            //    Debug.Log($"Match {klud?.name} ({klud?.transform.localPosition.x}, {klud?.transform.localPosition.y}) [{klud?.GetComponent<BustPiece>()?.color}] to {kludColor}: {matches}");
            //}
            return matches;
        }
    }
}
