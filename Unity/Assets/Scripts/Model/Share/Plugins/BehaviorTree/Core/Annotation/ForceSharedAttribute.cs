using System;
namespace BehaviorTree
{
    /// <summary>
    /// Force SharedVariable in the editor to use sharing mode
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class ForceSharedAttribute : Attribute
    {

    }
}
