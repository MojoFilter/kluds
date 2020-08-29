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
        //this.StartCoroutine(this.DropPeriodically());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dock(GameObject klud, Vector2 contactPoint)
    {
        this.board.Dock(klud, contactPoint);
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
}
