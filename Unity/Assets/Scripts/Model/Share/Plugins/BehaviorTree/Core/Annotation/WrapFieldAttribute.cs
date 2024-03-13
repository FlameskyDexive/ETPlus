using System;
namespace BehaviorTree
{
    /// <summary>
    /// Wrap field to use legacy IMGUI property field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class WrapFieldAttribute : Attribute
    {

    }
}
