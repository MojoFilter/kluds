using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class KludProvider : MonoBehaviour
{
    public List<GameObject> kludPrefabs = new List<GameObject>();

    public PrefabEvent nextKludChanged = new PrefabEvent();

    [NonSerialized]
    public GameObject nextKlud;

    private void Start()
    {
        this.Reload();
    }


    public GameObject TakePrefab()
    {
        var current = nextKlud;
        this.Reload();
        return current;
    }

    private void Reload()
    {
        this.nextKlud = this.kludPrefabs[Random.Range(0, kludPrefabs.Count)];
        this.nextKludChanged.Invoke(this.nextKlud);
    }
}

[Serializable]
public class PrefabEvent : UnityEvent<GameObject> { }
