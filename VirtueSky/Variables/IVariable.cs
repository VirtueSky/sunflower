namespace VirtueSky.Variables
{
    public interface IVariable<TType>
    {
        TType Value { get; set; }
    }
}