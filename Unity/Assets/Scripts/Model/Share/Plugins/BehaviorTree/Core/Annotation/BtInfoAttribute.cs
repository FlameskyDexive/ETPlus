using System;
namespace BehaviorTree
{
    /// <summary>
    /// Describe node behavior in the editor
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class BtInfoAttribute : Attribute
    {
        public string Description
        {
            get
            {
                return mDescription;
            }
        }

        private readonly string mDescription;
        public BtInfoAttribute(string description)
        {
            mDescription = description;
        }
    }
}