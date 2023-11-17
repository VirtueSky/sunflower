namespace VirtueSky.Attributes
{
    public partial class Button
    {
        public class BeginVerticalAttribute : ButtonBaseAttribute
        {
            public string text;

            public BeginVerticalAttribute()
            {
            }

            public BeginVerticalAttribute(string text)
            {
                this.text = text;
            }
        }
    }
}