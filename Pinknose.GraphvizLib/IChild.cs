namespace Pinknose.GraphvizLib
{
    public interface IParent
    {
        #region Properties

        List<GraphvizElement> Children { get; }

        Graph Graph { get; }

        #endregion Properties
    }
}