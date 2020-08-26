using Assets.Scripts.BustAKlud;
using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        //_board = new GameObject[columnCount, 32];
        this.StartCoroutine(this.DropPeriodically());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dock(Vector3 snapOrigin, GameObject kludObject)
    {
        this.board.Dock(snapOrigin, kludObject);
        
        //var kludTransform = klud.GetComponent<Transform>();
        //var pos = kludTransform.localPosition;
        //var left = pos.x;

        //// round to nearest width
        //var roundFactor = 1f / kludHalfWidth;
        //left -= kludHalfWidth;
        //left = Mathf.Round(left * roundFactor) / roundFactor;
        //left += kludHalfWidth;
        //left = Mathf.Clamp(left, kludHalfWidth, kludHalfWidth * 2f * columnCount + kludHalfWidth);
        //Debug.Log($"Shifted from {pos.x} to {left} ({roundFactor})");
        //kludTransform.localPosition = new Vector3(left, kludHalfHeight, pos.z);
    }

    //private GameObject[,] _board;

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
}
