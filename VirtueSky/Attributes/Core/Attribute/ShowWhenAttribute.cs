namespace VirtueSky.Attributes
{
    // DONT PUT IN EDITOR FOLDER

    using System;
    using UnityEngine;

    /// <summary>
    /// Attribute used to show or hide the Field depending on certain conditions
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowWhenAttribute : PropertyAttribute {

        public readonly string conditionFieldName;
        public readonly object comparationValue;
        public readonly object[] comparationValueArray;
	
        /// <summary>
        /// Attribute used to show or hide the Field depending on certain conditions
        /// </summary>
        /// <param name="conditionFieldName">Name of the bool condition Field</param>
        public ShowWhenAttribute(string conditionFieldName)
        {
            this.conditionFieldName = conditionFieldName;
        }
	
        /// <summary>
        /// Attribute used to show or hide the Field depending on certain conditions
        /// </summary>
        /// <param name="conditionFieldName">Name of the Field to compare (bool, enum, int or float)</param>
        /// <param name="comparationValue">Value to compare</param>
        public ShowWhenAttribute(string conditionFieldName, object comparationValue = null)
        {
            this.conditionFieldName = conditionFieldName;
            this.comparationValue = comparationValue;
        }
	
        /// <summary>
        /// Attribute used to show or hide the Field depending on certain conditions
        /// </summary>
        /// <param name="conditionFieldName">Name of the Field to compare (bool, enum, int or float)</param>
        /// <param name="comparationValueArray">Array of values to compare (only for enums)</param>
        public ShowWhenAttribute(string conditionFieldName, object[] comparationValueArray = null)
        {
            this.conditionFieldName = conditionFieldName;
            this.comparationValueArray = comparationValueArray;
        }
    }
}