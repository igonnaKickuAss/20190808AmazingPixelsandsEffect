using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLiOYouxi.OObjects
{
    using OLiOYouxi.OSystem;

    public class Sands : LivePixel
    {
        static public int gravity = -50;

        public Vector2 velocity;

        bool ClearAt(DrawerController drawer, int x, int y)
        {
            Color cs = drawer.GetPixel(x, y);

            return cs == Color.black || cs.a == 0;
        }

        //VAR
        bool clearLeft = false, clearRight = false;

        public override void Update(DrawerController drawer)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity *= 1f - 0.1f * Time.deltaTime;
            position += velocity * Time.deltaTime;

            if (x > renderWEnd || y > renderHEnd || y < renderHStart || x < renderWStart)
                Die();  //都跑地图外了，那就让他去死吧

            if (!ClearAt(drawer, x, y))
            {
                // 这个粒子碰到了什么东西，  如果是的话就判断是否清理两边。
                clearLeft = ClearAt(drawer, x - 1, y);
                clearRight = ClearAt(drawer, x + 1, y);
                if (clearLeft && clearRight)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f) position.x -= 1;
                    else position.x += 1;
                }
                else if (clearLeft)
                {
                    position.x -= 1;
                }
                else if (clearRight)
                {
                    position.x += 1;
                }
                else if (velocity.y < 0)
                {
                    // 他正在下坠，如果没发现找到符合清理要求的地方，那就网上移动一格，然后就去死吧
                    position.y += 1;
                    Die();
                }
                velocity = Vector2.zero;
            }
        }
    }

    public class Water : LivePixel
    {
        static public int gravity = -100;
        static EnumCentre.BrushType attach = EnumCentre.BrushType.Default;

        public Vector2 velocity;

        bool ClearAt(DrawerController drawer, int x, int y)
        {
            Color cs = drawer.GetPixel(x, y);

            return cs == Color.black || cs.a == 0;
        }

        bool Dieposition(DrawerController drawer, int x, int y)
        {
            Color cs = drawer.GetPixel(x, y);

            return cs == color;
        }

        void CreateNewSpurting(DrawerController drawer)
        {
            position.y -= 1;
            Vector2 velo = Vector2.zero;
            HelperAPIMethods.RandomVelocity(ref velo, -1f, 1f, 0, 1f);
            drawer.CreateLivePixel<Spurting>(position, attach).velocity = 20f * velo;     //溅起水花
        }

        //VAR
        bool clearLeft, clearRight;
        float lifeTime;

        Vector2 left = new Vector2(-30f, 13f);
        Vector2 right = new Vector2(30f, 13f);

        public override void Start(DrawerController drawer)
        {
            clearLeft = false; clearRight = false;
            lifeTime = Time.time + 1f;
        }

        public override void Update(DrawerController drawer)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity *= 1f - 0.1f * Time.deltaTime;
            position += velocity * Time.deltaTime;

            if (x > renderWEnd || y > renderHEnd || y < renderHStart || x < renderWStart)
                Die();  //都跑地图外了，那就让他去死吧

            if (!ClearAt(drawer, x, y))
            {
                // 这个粒子碰到了什么东西，  如果是的话就判断是否清理两边。
                clearLeft = ClearAt(drawer, x - 1, y);
                clearRight = ClearAt(drawer, x + 1, y);
                if (clearLeft && clearRight)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                    {
                        position.x -= 1;
                        left.Set(-30f, UnityEngine.Random.Range(1f, 20f));
                        velocity = left;
                        return;
                    }
                    position.x += 1;
                    right.Set(30f, UnityEngine.Random.Range(1f, 20f));
                    velocity = right;
                    return;
                }
                else if (clearLeft)
                {
                    position.x -= 1;
                    left.Set(-30f, UnityEngine.Random.Range(1f, 20f));
                    velocity = left;
                    return;
                }
                else if (clearRight)
                {
                    position.x += 1;
                    right.Set(30f, UnityEngine.Random.Range(1f, 20f));
                    velocity = right;
                    return;
                }
                else if (velocity.y < 0)
                {
                    //注意，偶买一哇木，西内得一路！
                    if (Time.time > lifeTime)
                    {
                        if (Dieposition(drawer, x, y))
                        {
                            CreateNewSpurting(drawer);

                            velocity = Vector2.zero;
                            DieClear();
                            return;
                        }
                        Die();      //大限已到，去死吧
                        return;
                    }
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                    {
                        position.x -= UnityEngine.Random.Range(2, 9);
                        left.Set(-30f, UnityEngine.Random.Range(1f, 20f));
                        velocity = left;
                        return;
                    }
                    position.x += UnityEngine.Random.Range(2, 9);
                    right.Set(30f, UnityEngine.Random.Range(1f, 20f));
                    velocity = right;
                    return;

                }


            }
        }
    }

    public class Fire : LivePixel
    {
        static public float bouyancy = 30f;     //浮力值，模仿跳动的火焰
        static public float lifeTime = 1f;
        static EnumCentre.BrushType brush = EnumCentre.BrushType.Fire;
        static EnumCentre.BrushType attach = EnumCentre.BrushType.Default;

        //VAR
        float startTime;

        public override void Start(DrawerController drawer)
        {
            startTime = Time.time;
            color = MakeFireInInspector.instance.fireStartColor;
        }

        //VAR
        Vector2 pos = Vector2.zero;
        public override void Update(DrawerController drawer)
        {
            //调整位置和粒子的颜色
            position.y += bouyancy * Time.deltaTime;

            //判断粒子当前的“年龄”，毕竟各个阶段的火焰颜色不同嘛
            float t = (Time.time - startTime) / lifeTime;

            //根据他的年龄，给他上色
            color = Color.Lerp(MakeFireInInspector.instance.fireStartColor, MakeFireInInspector.instance.fireEndColor, t);
            if (t > 1)
                DieClear();     //大限已到，节哀

            //时不时生一个活的火焰
            if (UnityEngine.Random.Range(0, 100) < 10)
            {
                pos.Set(x, y - 1);
                drawer.CreateLivePixel<Fire>(pos, brush).startTime = startTime - .3f;
            }

            //如果碰到了木头，那就把他烧成灰烬人(生点余烬粒子)，还有足够时间的着火才有几率点燃噢
            Color c = drawer.GetPixel(x, y, false);
            if (MakeFireInInspector.instance.IsFlammable(c) && UnityEngine.Random.Range(0, 100) < 2)
            {
                pos.Set(x, y);
                drawer.CreateLivePixel<Ember>(pos, attach);
            }

            //如果碰到雪了。。
            if (c.IsGrayscale())
                drawer.SetPixel(x, y, Color.clear);

            //碰到雨了
            if (c.IsBluescale())
                drawer.SetPixel(x, y, Color.clear);

        }


        static public void CreateFlameAt(DrawerController drawer, Vector2 pos)
        {
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i == j || -i == j) && i != 0) continue;

                    pos.Set(x + i, y + j);
                    drawer.CreateLivePixel<Fire>(pos, brush);
                }
            }
        }
    }

    public class Corals : LivePixel
    {
        static public int gravity = -50;

        public Vector2 velocity;

        bool ClearAt(DrawerController drawer, int x, int y)
        {
            Color cs = drawer.GetPixel(x, y);

            return cs == Color.black || cs.a == 0;
        }

        //VAR
        bool clearLeft = false, clearRight = false;

        public override void Update(DrawerController drawer)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity *= 1f - 0.1f * Time.deltaTime;
            position += velocity * Time.deltaTime;

            if (x > renderWEnd || y > renderHEnd || y < renderHStart || x < renderWStart)
                Die();  //都跑地图外了，那就让他去死吧

            if (!ClearAt(drawer, x, y))
            {
                // 这个粒子碰到了什么东西，  如果是的话就判断是否清理两边。
                clearLeft = ClearAt(drawer, x - 1, y);
                clearRight = ClearAt(drawer, x + 1, y);
                if (clearLeft && clearRight)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f) position.x -= 1;
                    else position.x += 1;
                }
                else if (clearLeft)
                {
                    position.x -= 1;
                    Die();
                }
                else if (clearRight)
                {
                    position.x += 1;
                    Die();
                }
                else if (velocity.y < 0)
                {
                    // 他正在下坠，如果没发现找到符合清理要求的地方，那就网上移动一格，然后就去死吧
                    position.y += 1;
                    Die();
                }
                velocity = Vector2.zero;
            }
        }
    }
    
    public class Rain : LivePixel
    {
        static public int gravity = -80;
        static EnumCentre.BrushType attach = EnumCentre.BrushType.Default;

        public Vector2 velocity;

        bool ClearAt(DrawerController drawer, int x, int y)
        {
            Color cs = drawer.GetPixel(x, y);

            return cs == Color.black || cs.a == 0;
        }

        //VAR
        bool clearLeft = false, clearRight = false;

        Vector2 left = new Vector2(-20f, 0);
        Vector2 right = new Vector2(20f, 0);

        public override void Update(DrawerController drawer)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity *= 1f - 0.1f * Time.deltaTime;
            position += velocity * Time.deltaTime;

            if (x > renderWEnd || y > renderHEnd || y < renderHStart || x < renderWStart)
                Die();  //都跑地图外了，那就让他去死吧

            if (!ClearAt(drawer, x, y))
            {
                // 这个粒子碰到了什么东西，  如果是的话就判断是否清理两边。
                clearLeft = ClearAt(drawer, x - 1, y);
                clearRight = ClearAt(drawer, x + 1, y);
                if (clearLeft && clearRight)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                    {
                        position.x -= 1;
                        velocity = left;

                        return;
                    }
                    else
                    {
                        position.x += 1;
                        velocity = right;

                        return;
                    }

                }
                else if (clearLeft)
                {
                    position.x -= 1;
                    velocity = left;
                    return;
                }
                else if (clearRight)
                {
                    position.x += 1;
                    velocity = right;
                    return;
                }
                else if (velocity.y < 0)
                {
                    // 他正在下坠，如果没发现找到符合清理要求的地方，那就网上移动一格，然后就去死吧
                    position.y += 1;
                    DieClear();
                    Vector2 velo = Vector2.zero;
                    HelperAPIMethods.RandomVelocity(ref velo, -1f, 1f, 0, 1f);
                    drawer.CreateLivePixel<Spurting>(position, attach).velocity = 20f * velo;
                }
                velocity = Vector2.zero;
            }
        }
    }

    public class Snow : LivePixel
    {
        bool ClearAt(DrawerController drawer, int x, int y)
        {
            Color c = drawer.GetPixel(x, y);
            return c == Color.black || c.a == 0;
        }

        public override void Update(DrawerController drawer)
        {
            float r = UnityEngine.Random.Range(0.5f, 1f);
            color.r = r;
            color.g = r;
            color.b = r;

            int oldy = y;
            position.y -= Time.deltaTime * 10f;
            if (y != oldy) position.x += UnityEngine.Random.Range(-1f, 1f);

            if (!ClearAt(drawer, x, y))
            {
                bool clearLeft = ClearAt(drawer, x - 1, y);
                bool clearRight = ClearAt(drawer, x + 1, y);
                if (clearLeft && clearRight)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f) position.x--;
                    else position.x++;
                }
                else if (clearLeft)
                {
                    position.x--;
                }
                else if (clearRight)
                {
                    position.x++;
                }
                else
                {
                    position.y++;
                    Die();
                }
            }
        }
    }

    #region -- 非笔刷类型的粒子 --

    /// <summary>
    /// 这是溅射粒子
    /// </summary>
    public class Spurting : LivePixel
    {
        static public int gravity = -40;

        public Vector2 velocity;

        bool ClearAt(DrawerController drawer, int x, int y)
        {
            Color cs = drawer.GetPixel(x, y);

            return cs == Color.black || cs.a == 0;
        }

        //VAR
        bool clearLeft = false, clearRight = false;

        Vector2 left = new Vector2(-20f, 0);
        Vector2 right = new Vector2(20f, 0);

        public override void Start(DrawerController drawer)
        {
            //初始化
            color.r = 0;
            color.g = UnityEngine.Random.Range(.4f, .8f);
            color.b = 1f;

        }

        public override void Update(DrawerController drawer)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity *= 1f - 0.1f * Time.deltaTime;
            position += velocity * Time.deltaTime;

            if (x > renderWEnd || y > renderHEnd || y < renderHStart || x < renderWStart)
                Die();  //都跑地图外了，那就让他去死吧

            if (!ClearAt(drawer, x, y))
            {
                // 这个粒子碰到了什么东西，  如果是的话就判断是否清理两边。
                clearLeft = ClearAt(drawer, x - 1, y);
                clearRight = ClearAt(drawer, x + 1, y);
                if (clearLeft && clearRight)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                    {
                        position.x -= 1;
                        velocity = left;

                        return;
                    }
                    else
                    {
                        position.x += 1;
                        velocity = right;

                        return;
                    }

                }
                else if (clearLeft)
                {
                    position.x -= 1;
                    velocity = left;
                    return;
                }
                else if (clearRight)
                {
                    position.x += 1;
                    velocity = right;
                    return;
                }
                else if (velocity.y < 0)
                {
                    // 他正在下坠，如果没发现找到符合清理要求的地方，那就网上移动一格，然后就去死吧
                    position.y += 1;
                    DieClear();
                }
                velocity = Vector2.zero;
            }
        }
    }

    /// <summary>
    /// 传火灰烬，余烬嘿嘿
    /// </summary>
    public class Ember : LivePixel
    {
        float nextFireTime;     //下次着火时间
        float dieTime;      //啥时候死掉

        public override void Start(DrawerController drawer)
        {
            //初始化
            dieTime = Time.time + UnityEngine.Random.Range(.1f, 2f);

            color.r = 1f;
            color.g = UnityEngine.Random.Range(0f, .5f);
            color.b = 0;
            drawer.SetPixel(x, y, Color.black);
            nextFireTime = Time.time + UnityEngine.Random.Range(0, .4f);
        }

        public override void Update(DrawerController drawer)
        {
            //已经到了下次着火时间
            if (Time.time > nextFireTime)
            {
                Fire.CreateFlameAt(drawer, position + UnityEngine.Random.insideUnitCircle * 2f);
                nextFireTime += .4f;
            }
            //大限已到
            if (Time.time > dieTime)
            {
                drawer.SetPixel(x, y, Color.clear);   //木头已经被成灰烬人了。。
                DieClear();
            }
        }
    }

    /// <summary>
    /// 这是渣滓粒子
    /// </summary>
    public class Dross : LivePixel
    {
        static public int gravity = -50;

        public Vector2 velocity;

        bool ClearAt(DrawerController drawer, int x, int y)
        {
            Color cs = drawer.GetPixel(x, y);

            return cs == Color.black || cs.a == 0;
        }

        //VAR
        bool clearLeft = false, clearRight = false;

        public override void Start(DrawerController drawer)
        {
            color = drawer.GetPixel(x, y);
        }

        public override void Update(DrawerController drawer)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity *= 1f - 0.8f * Time.deltaTime;
            position += velocity * Time.deltaTime;

            if (x > renderWEnd || y > renderHEnd || y < renderHStart || x < renderWStart)
                Die();  //都跑地图外了，那就让他去死吧

            if (!ClearAt(drawer, x, y))
            {
                // 这个粒子碰到了什么东西，  如果是的话就判断是否清理两边。
                clearLeft = ClearAt(drawer, x - 1, y);
                clearRight = ClearAt(drawer, x + 1, y);
                if (clearLeft && clearRight)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f) position.x -= 1;
                    else position.x += 1;
                }
                else if (clearLeft)
                {
                    position.x -= 1;
                }
                else if (clearRight)
                {
                    position.x += 1;
                }
                else if (velocity.y < 0)
                {
                    // 他正在下坠，如果没发现找到符合清理要求的地方，那就网上移动一格，然后就去死吧
                    position.y += 1;
                    Die();
                }
                velocity = Vector2.zero;
            }
        }
    }

    #endregion

}