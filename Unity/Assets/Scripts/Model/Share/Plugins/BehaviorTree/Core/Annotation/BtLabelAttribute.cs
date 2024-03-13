using System;
namespace BehaviorTree
{
    /// <summary>
    /// Replace the name of the behavior node in the editor, or replace the field name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class BtLabelAttribute : Attribute
    {
        public string Title
        {
            get
            {
                return mTitle;
            }
        }

        private readonly string mTitle;
        public BtLabelAttribute(string tite)
        {
            mTitle = tite;
        }
    }

}