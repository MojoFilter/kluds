using UnityEngine;

public class NextKludPreview : MonoBehaviour
{
    public void OnNextKludChanged(GameObject kludPrefab)
    {
        var previewKlud = Instantiate(kludPrefab);
        previewKlud.transform.SetParent(this.transform);
        previewKlud.transform.localPosition = Vector3.zero;
        previewKlud.transform.localScale = Vector3.one;
    }
}
