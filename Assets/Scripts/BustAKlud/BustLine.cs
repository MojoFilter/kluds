using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BustAKlud
{
    public class BustLine : List<GameObject>
    {
        public BustLine(bool isShort)
        {
            this.IsShortLine = isShort;
        }

        public bool IsShortLine { get; set; }
    }
}
