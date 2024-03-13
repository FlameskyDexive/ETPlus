using System;
namespace BehaviorTree
{
    /// <summary>
    /// Nodes are categorized in the editor dropdown menu, and can be sub-categorized with the '/' symbol
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class BtGroupAttribute : Attribute
    {
        public string Group
        {
            get
            {
                return this.mGroup;
            }
        }

        private readonly string mGroup;
        public BtGroupAttribute(string group)
        {
            this.mGroup = group;
        }
    }

}