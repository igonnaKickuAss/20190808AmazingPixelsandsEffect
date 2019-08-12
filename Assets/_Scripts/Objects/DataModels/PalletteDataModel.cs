using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    public class PalletteDataModel
	{
        public GameObject[,] tileObjs { get; set; }
        public Texture2D[,] tileTexs { get; set; }
        public Texture2D_GPU[,] tileTexsGPU { get; set; }
        public Material[,] tileMats { get; set; }
        public bool[,] tileNeedsApply { get; set; }

    }
}