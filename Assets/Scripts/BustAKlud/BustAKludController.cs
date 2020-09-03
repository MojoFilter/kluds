using Assets.Scripts.BustAKlud;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class BustAKludController : MonoBehaviour
{
    public float kludHalfHeight = -0.25f;
    public float kludHalfWidth = 0.2f;
    public int columnCount = 6;

    public bool dropEnabled = true;
    public float dropPeriodSeconds = 4f;

    public Crusher crusher;

    public BustBoard board;

    public KludProvider provider;

    private void Awake()
    {
        this.LoadLevel();
    }

    public void Dock(GameObject klud)
    {
        this.board.Dock(klud);
    }


    private IEnumerator DropPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.dropPeriodSeconds);
            if (this.dropEnabled)
            {
                this.crusher.Drop();
            }
        }
    }

    private void LoadLevel()
    {
        var data = Resources.Load<TextAsset>("level01");
        using (var reader = new StringReader(data.text))
        {
            //try
            //{
            var longColumns = int.Parse(reader.ReadLine());
            var rows = int.Parse(reader.ReadLine());

            this.board.longRowWidth = longColumns;
            this.board.maxLines = rows;

            var lineIndex = 0;
            while (reader.ReadLine() is var line && !string.IsNullOrWhiteSpace(line))
            {
                var pieces =
                    line.Where(char.IsDigit)
                        .Select((c, i) => (col: i, val: (int)char.GetNumericValue(c)))
                        .Where(k => k.val > 0)
                        .Select(k => (k.col, klud: provider.kludPrefabs[k.val - 1]));
                foreach (var piece in pieces)
                {
                    this.board.PlaceKlud(lineIndex, piece.col, piece.klud);
                }
                lineIndex++;
            }
            //} 
            //catch(Exception ex)
            //{
            //    Debug.LogError(ex);
            //}
        }
    }
}
