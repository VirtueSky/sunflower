namespace VirtueSky.Attributes
{
    public partial class ButtonAttribute : ButtonBaseAttribute
    {
        public string text;


        public ButtonAttribute()
        {
        }

        public ButtonAttribute(string buttonName)
        {
            this.text = buttonName;
        }
    }
}