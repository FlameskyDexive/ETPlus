using System;
namespace BehaviorTree
{
    /// <summary>
    /// Disable copying of field's value within the editor
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class CopyDisableAttribute : Attribute
    {

    }
}
