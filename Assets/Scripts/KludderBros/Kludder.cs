using System.Collections.Generic;
using UnityEngine;

public class Kludder : MonoBehaviour
{
	public float minimumScale = 0.3f;
	public GameObject kludderPrefab;
	public List<Sprite> kludderSprites = new List<Sprite>();
	public Vector2 startForce = new Vector2(2f, 5f);

    public void Split()
    {
		var nextScale = this.transform.localScale.x / 2f;
		if (nextScale >= minimumScale)
		{
			var sprite = kludderSprites[Random.Range(0, kludderSprites.Count)];
			Vector2 basePosition = this.transform.position;
			GameObject ball1 = Instantiate(kludderPrefab, basePosition + Vector2.right / 4f, Quaternion.identity, this.transform.parent);
			GameObject ball2 = Instantiate(kludderPrefab, basePosition + Vector2.left / 4f, Quaternion.identity, this.transform.parent);

			var scale = new Vector3(nextScale, nextScale, 1f);
			ball1.transform.localScale = scale;
			ball2.transform.localScale = scale;

			ball1.GetComponent<SpriteRenderer>().sprite = sprite;
			ball2.GetComponent<SpriteRenderer>().sprite = sprite;

			ball1.GetComponent<Kludder>().startForce = new Vector2(2f, 5f);
			ball2.GetComponent<Kludder>().startForce = new Vector2(-2f, 5f);
		}
		else
        {
			Debug.LogError($"Ended at scale {this.transform.localScale.x} => {nextScale} / {minimumScale}");
        }
		Destroy(this.gameObject);
	}

    private void Start()
    {
		this.GetComponent<Rigidbody2D>().AddForce(this.startForce, ForceMode2D.Impulse);
    }
}

