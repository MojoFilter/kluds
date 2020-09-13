using Assets.Scripts.BustAKlud;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class KludProvider : MonoBehaviour
{
    public BustBoard board;

    public List<GameObject> kludPrefabs = new List<GameObject>();

    public PrefabEvent nextKludChanged = new PrefabEvent();

    [NonSerialized]
    public GameObject nextKlud;

    private Dictionary<Color, GameObject> _kludTable;

    private void Awake()
    {
        _kludTable = kludPrefabs.ToDictionary(p => p.GetComponent<BustPiece>().color);
    }


    private void Start()
    {
        this.Reload();
    }


    public GameObject TakePrefab()
    {
        var current = nextKlud;
        this.nextKlud = null;
        this.FireNextKludChanged();
        return current;
    }

    public void Reload()
    {
        var colorsInPlay = board.GetKludTypesInPlay().ToList();
        var nextColor = colorsInPlay[Random.Range(0, colorsInPlay.Count)];
        this.nextKlud = _kludTable[nextColor];
        this.FireNextKludChanged();
    }

    private void FireNextKludChanged()
    {
        this.nextKludChanged.Invoke(this.nextKlud);
    }
}

[Serializable]
public class PrefabEvent : UnityEvent<GameObject> { }
