using System;
using System.Collections;
using System.Collections.Generic;

namespace OLiOYouxi.OSystem
{
    /// <summary>
    /// 为了避免值类型到引用类型的装箱拆箱的操作，重写List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NoGCList<T> : IEnumerable<T>
    {
        #region -- Protected Data --
        protected T[] values;

        protected int capacity = 10; //容量
        protected int length;// 存放的单元个数
        protected int mayIdleId;// 可能空闲的单元下标

        protected IEnumerator<T>[] enumerators;
        protected bool[] enumStates;     //枚举器组当前占用状态


        #endregion

        #region -- Public ShotC --
        public virtual int Count
        {
            get
            {
                return length;
            }
        }

        #endregion


        #region -- 初始化 --
        public NoGCList(int capacity, int enumCount)
        {
            this.capacity = capacity;
            Init(capacity);
        }

        protected void Init(int enumCount)
        {
            values = new T[this.capacity];

            if (enumerators == null)
            {
                enumerators = new IEnumerator<T>[enumCount];
                enumStates = new bool[enumCount];
                for (int i = 0; i < enumerators.Length; i++)
                {
                    enumerators[i] = new NoGCListEnumerator<T>(this, i);
                }
            }
        }


        #endregion


        public IEnumerator<T> GetEnumerator()
        {
            //搜索可用的枚举器
            int idleEnumId = -1;
            for (int i = 0; i < enumStates.Length; i++)
            {
                int tryID = i + mayIdleId;
                if (!enumStates[tryID]) //这个枚举器处于未占用状态
                {
                    idleEnumId = tryID;
                    break;
                }
            }

            //标记他为正在使用，别忘记初始化这个枚举器
            enumStates[idleEnumId] = true;
            enumerators[idleEnumId].Reset();

            //向前移动空闲坐标
            mayIdleId = (mayIdleId + 1) % enumStates.Length;


            return enumerators[idleEnumId];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return null;
        }

        #region -- IEnumerator --
        /// <summary>
        /// 迭代器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        struct NoGCListEnumerator<T> : IDisposable, IEnumerator<T>
        {
            private NoGCList<T> parent;
            private T value;

            private int position;
            private int id;

            internal NoGCListEnumerator(NoGCList<T> list, int id)
            {
                this.parent = list;
                this.value = default(T);
                this.position = -1;
                this.id = id;
            }



            T IEnumerator<T>.Current
            {
                get
                {
                    return value;   //根据下标返回数组中的某元素
                }
            }

            public object Current
            {
                get
                {
                    if (position == -1 || position == parent.values.Length) //第一个之前和最后一个自后的访问非法
                    {
                        throw new InvalidOperationException();
                    }

                    return value;        //根据下标返回数组中的某元素
                }
            }

            public void Dispose()
            {
                //this.m_list = null;
                //清除使用标记
                parent.enumStates[id] = false;
                parent.mayIdleId = id;
            }

            public bool MoveNext()
            {
                if (position != parent.values.Length)
                {
                    position++;
                }

                if (position < 0)
                {
                    return false;
                }

                if (position < parent.Count)
                {
                    value = parent.values[position];
                    return true;
                }

                position = -1;
                return false;
            }

            public void Reset()
            {
                position = -1;
            }

            #region -- default --
            //public object Current
            //{
            //    get
            //    {
            //        if (position == -1 || position == parent.values.Length) //第一个之前和最后一个自后的访问非法
            //        {
            //            throw new InvalidOperationException();
            //        }
            //        int index = position + parent.startingPoint;
            //        index = index % parent.values.Length;
            //        return parent.values[index];        //根据下标返回数组中的某元素
            //    }
            //}



            //public bool MoveNext()
            //{
            //    if (position != parent.values.Length)
            //    {
            //        position++;
            //    }
            //    return position < parent.values.Length;
            //}

            //public void Reset()
            //{
            //    position = -1;
            //}
            #endregion

        }

        #endregion



        public virtual void Add(T element)
        {
            IncreaseCapacity();

            //赋值
            values[length] = element;
            length++;
        }

        /// <summary>
        /// 此功能未实现
        /// </summary>
        /// <param name="elements"></param>
        public virtual void AddRange(T[] elements)
        {
            //TODO..未实现
            IncreaseCapacity();
        }

        /// <summary>
        /// 清空数组元素
        /// </summary>
        public virtual void Clear()
        {
            for (int i = 0; i < length; i++)
            {
                values[i] = default(T);
            }
            length = 0;
        }

        public virtual T this[int index]
        {
            get
            {
                //取得某个位置上数组元素
                if (index < 0 || index >= length)
                    throw new InvalidOperationException();

                return values[index];
            }
            set
            {
                //设置某个位置上的数组元素
                if (index < 0 || index >= length)
                    throw new InvalidOperationException();

                values[index] = value;
            }
        }

        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= length)
                return;

            for (int i = index; i <= length - 2; i++)
            {
                values[i] = values[i + 1];
            }
            length--;
        }


        protected void IncreaseCapacity()
        {
            if (length >= this.capacity)
            {
                int newCapacity = capacity;
                if (newCapacity == 0)
                    newCapacity++;

                newCapacity *= 2;
                T[] newValues = new T[newCapacity];
                Array.Copy(values, 0, newValues, 0, length);
                values = newValues;
                capacity = newCapacity;
            }
        }

    }

}