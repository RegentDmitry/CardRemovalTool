using System;

namespace CardRemovalTool
{
    [AttributeUsage(AttributeTargets.Field)]
    class TextValueAttribute : Attribute
    {
        public string Text { get; private set; }
        public TextValueAttribute(string text)
        {
            Text = text;
        }
    }

    static class EnumExtensions
    {
        public static string GetText(this Enum e)
        {
            var attribs = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(TextValueAttribute), false);
            return attribs.Length > 0 ? ((TextValueAttribute) attribs[attribs.Length - 1]).Text : e.ToString();
        }

    }
}
