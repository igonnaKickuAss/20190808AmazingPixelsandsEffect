namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 关于组的抽象类
    /// </summary>
    public abstract class AGroupAttribute : OLiOYouxiAttribute
    {
        public string Name { get; private set; }

        public AGroupAttribute(string name)
        {
            this.Name = name;
        }
    }
}