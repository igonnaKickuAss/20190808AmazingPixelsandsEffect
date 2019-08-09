using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
	public class LivePixel 
	{
        public Vector2 position;
        public int x { get { return Mathf.RoundToInt(position.x); } }
        public int y { get { return Mathf.RoundToInt(position.y); } }

        public int lastUpdateFrame;


        public float renderWStart;
        public float renderHStart;
        public float renderWEnd;
        public float renderHEnd;

        public Color color;
        public bool isDead;

        public virtual void Start(DrawerController drawer) { }

        public virtual void Update(DrawerController drawer) { }

        public void Die()
        {
            isDead = true;
        }

        public void DieClear()
        {
            //TODO..让他死掉!!!!!
            //TODO..让他消失!!!     就是使用透明纹理 color.clear
            //Start..
            isDead = true;
            color = Color.clear;
        }


    }
}