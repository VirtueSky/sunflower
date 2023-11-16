namespace VirtueSky.Variables
{
    public interface IReference
    {
    }

    public interface IReference<TType, TVariable> : IReference
    {
        TType Value { get; set; }
    }
}