using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGrid : MonoBehaviour
{
    public int rows = 12;
    public int longRowWidth = 10;

    public GameObject slotPrefab;

    private void Awake()
    {
        for (int i = 0; i < rows; ++i)
        {
            var rowWidth = this.longRowWidth;
            var rowStart = 0f;
            if (i % 2 == 1)
            {
                rowWidth--;
                rowStart = 0.5f;
            }
            for (float k = rowStart; k < rowWidth; ++k)
            {
                Instantiate(slotPrefab, this.transform).transform.localPosition = new Vector3(k, -i);
            }
        }
    }
}
