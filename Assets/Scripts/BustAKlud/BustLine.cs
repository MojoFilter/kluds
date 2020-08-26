using System.Collections.Generic;

namespace Assets.Scripts.BustAKlud
{
    public class BustLine : List<BustPiece>
    {
        public BustLine(bool isShort)
        {
            this.IsShortLine = isShort;
        }

        public bool IsShortLine { get; set; }
    }
}
