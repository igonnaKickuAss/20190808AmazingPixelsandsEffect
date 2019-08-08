namespace OLiOYouxiAttributes.Editor
{
    /// <summary>
    /// 关于property组绘制的抽象类
    /// </summary>
	public abstract class APropertyGrouper
	{
        public abstract void BeginGroup(string label);
        public abstract void EndGroup();

	}
}