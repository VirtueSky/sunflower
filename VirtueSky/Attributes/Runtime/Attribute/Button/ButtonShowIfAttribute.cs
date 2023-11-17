namespace VirtueSky.Attributes
{
    public class ButtonShowIfAttribute : ButtonBaseAttribute
    {
        public string text;
        public readonly string conditionFieldName;
        public readonly object comparationValue;
        public readonly object[] comparationValueArray;

        public ButtonShowIfAttribute(string buttonName, string conditionFieldName, object comparationValue)
        {
            this.text = buttonName;
            this.conditionFieldName = conditionFieldName;
            this.comparationValue = comparationValue;
        }

        public ButtonShowIfAttribute(string buttonName, string conditionFieldName, object[] comparationValueArray = null)
        {
            this.text = buttonName;
            this.conditionFieldName = conditionFieldName;
            this.comparationValueArray = comparationValueArray;
        }
    }
}