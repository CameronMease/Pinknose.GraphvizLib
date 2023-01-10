namespace Pinknose.GraphvizLib
{
    public interface IParent
    {
        #region Properties

        HashSet<GraphvizElement> Children { get; }

        Graph Graph { get; }

        #endregion Properties
    }
}