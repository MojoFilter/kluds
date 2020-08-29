using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestKlud : MonoBehaviour
{
    const float segmentLength = Mathf.PI * 2f / 6f;
    string[] segmentNames = new[]
        {
            "Upper-right",
            "Upper-left",
            "Left",
            "Lower-left",
            "Lower-Right",
            "Right"
        };

    Vector2[] segmentOffsets = new[]
    {
            (-.5f, -1f),
            (.5f, -1f),
            (.5f, 0f),
            (.5f, .5f),
            (-.5f, .5f),
            (-.5f, 0f)
        }.Select(i => new Vector2(i.Item1, i.Item2)).ToArray();

    private bool set = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!set && collision.otherCollider.gameObject.GetComponent<KludSector>() is KludSector sector)
        {
            set = true;
            var otherPos = collision.transform.localPosition;
            var pos = otherPos + sector.offset;
            Debug.Log($"Hit {collision.otherCollider.name} Set position {pos} -- {otherPos} / {sector.offset}");
        }
        //var contacts = new List<ContactPoint2D>();
        //collision.GetContacts(contacts);
        //var contactWorldPoint = contacts.Select(c => c.point).OrderBy(p => p.y).First();
        //var contactPoint = this.transform.InverseTransformPoint(contactWorldPoint);
        ////var relativeContactPoint = contactPoint - new Vector3(.5f, -.5f);
        //var hitRads = Mathf.Atan2(contactPoint.y, contactPoint.x);

        //var averageAngle = contacts.Select(c => c.point).Select(p => Mathf.Atan2(p.y, p.x)).Average() * Mathf.Rad2Deg;
        //var sector = Mathf.FloorToInt(((averageAngle - 30) % 360) / 60);

        //Debug.LogWarning($"Sector length: {segmentLength}r  {segmentLength * Mathf.Rad2Deg}deg");
        //Debug.Log($"Total contacts: {contacts.Count()} -- Lowest angle: {hitRads * Mathf.Rad2Deg} Avg angle: {averageAngle}");
        //Debug.LogWarning($"Hit in sector {sector} from {hitRads}");
        //Vector3 dock = collision.transform.localPosition + (Vector3)segmentOffsets[sector];
        ////this.transform.localPosition = dock;
        //Debug.Log($"Hit {collision.gameObject.name} at ({collision.transform.localPosition}) and docked at {transform.localPosition}");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this.set = false;
    }
}
