using UnityEngine;

public class BustACursor : MonoBehaviour
{
    public Camera camera;
    public Texture2D texture;
    private KludControls _controls;

    private void Awake()
    {
        _controls = new KludControls();
        //Cursor.visible = false;
        //Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        var screenPoint = _controls.Bust.Point.ReadValue<Vector2>();
        var worldPoint = this.camera.ScreenToWorldPoint(screenPoint);
        worldPoint.z = 0f;
        this.transform.position = worldPoint;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
