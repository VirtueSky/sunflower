using VirtueSky.Inspector;
using VirtueSky.Inspector.Drawers;
using VirtueSky.Inspector.Utilities;
using UnityEngine;

[assembly: RegisterTriAttributeDrawer(typeof(IndentDrawer), TriDrawerOrder.Decorator)]

namespace VirtueSky.Inspector.Drawers
{
    public class IndentDrawer : TriAttributeDrawer<IndentAttribute>
    {
        public override void OnGUI(Rect position, TriProperty property, TriElement next)
        {
            using (var indentedRectScope = TriGuiHelper.PushIndentedRect(position, Attribute.Indent))
            {
                next.OnGUI(indentedRectScope.IndentedRect);
            }
        }
    }
}