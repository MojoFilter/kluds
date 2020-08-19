using System.IO;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject gameboard;
    public GameObject barrel;
    public Animator smokeAnimator;
    public GameObject kludPrefab;
    public GameObject kludStart;

    public float kludSpeed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var target = this.mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.transform.position.z));
        var difference = target - this.transform.position;
        var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        this.barrel.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        if (Input.GetMouseButtonDown(0))
        {
            this.smokeAnimator.SetTrigger("Blast");
            var distance = difference.magnitude;
            Vector2 direction = difference / distance;
            direction.Normalize();
            this.FireKlud(direction, rotationZ);
        }
    }

    private void FireKlud(Vector2 direction, float rotationZ)
    {
        var klud = Instantiate(this.kludPrefab) as GameObject;
        klud.transform.parent = this.gameboard.transform;
        klud.transform.position = this.kludStart.transform.position;
        Debug.Log(klud.transform.position);
        //klud.transform.position.Set(klud.transform.position.x, klud.transform.position.y, -1);
        //klud.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        Debug.Log(klud.transform.position);
        klud.GetComponent<Rigidbody2D>().velocity = direction * this.kludSpeed;
    }
}
