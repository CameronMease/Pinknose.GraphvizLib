namespace Pinknose.GraphvizLib.Attributes
{
    internal interface IAttribute<T>
    {
        #region Properties

        public string Name { get; }

        public T Value { get; set; }

        #endregion Properties
    }
}