using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
	public class PallettePrevDataModel
	{
        public int prevTileWidth { get; set; }
        public int prevTilesCols { get; set; }
        public int prevTileHeight { get; set; }
        public int prevTilesRows { get; set; }
        public int prevTotalWidth { get; set; }
        public int prevTotalHeight { get; set; }
        public Vector2 prevRenderStartEndW { get; set; }
        public Vector2 prevRenderStartEndH { get; set; }
        public float prevPixelsPerUnit { get; set; }
        public bool prevStartWithTestPattern { get; set; }
    }
}