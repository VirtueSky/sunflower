using VirtueSky.Inspector;
using VirtueSky.Inspector.Elements;
using VirtueSky.Inspector.GroupDrawers;

[assembly: RegisterTriGroupDrawer(typeof(TriFoldoutGroupDrawer))]

namespace VirtueSky.Inspector.GroupDrawers
{
    public class TriFoldoutGroupDrawer : TriGroupDrawer<DeclareFoldoutGroupAttribute>
    {
        public override TriPropertyCollectionBaseElement CreateElement(DeclareFoldoutGroupAttribute attribute)
        {
            return new TriBoxGroupElement(new TriBoxGroupElement.Props
            {
                title = attribute.Title,
                titleMode = TriBoxGroupElement.TitleMode.Foldout,
                expandedByDefault = attribute.Expanded,
                hideIfChildrenInvisible = true,
            });
        }
    }
}