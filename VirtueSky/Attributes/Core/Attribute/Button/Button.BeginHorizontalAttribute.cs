namespace VirtueSky.Attributes
{
    public partial class Button
    {
        public class BeginHorizontalAttribute : ButtonBaseAttribute
        {
            public string text;

            public BeginHorizontalAttribute()
            {
            }

            public BeginHorizontalAttribute(string text)
            {
                this.text = text;
            }
        }
    }
}