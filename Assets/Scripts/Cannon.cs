using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject barrel;

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
    }
}
