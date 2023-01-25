using System;

namespace Pinknose.GraphvizLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class AttributeNameAttribute : Attribute
    {
        #region Constructors

        public AttributeNameAttribute(string name)
        {
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public string Name { get; }

        #endregion Properties
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class HeaderTextAttribute : Attribute
    {
        #region Constructors

        public HeaderTextAttribute(string text)
        {
            Text = text;
        }

        #endregion Constructors

        #region Properties

        public string Text { get; }

        #endregion Properties
    }
}