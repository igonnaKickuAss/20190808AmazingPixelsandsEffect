using System;

namespace OLiOYouxiAttributes
{
    /// <summary>
    /// 实现暴露一个你不想被public且是不能在面板编辑的字段，可是你又很想看他的值是多少（作用于字段）
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class NonSerializedFieldAttribute : ADrawerAttribute 
	{

	}
}