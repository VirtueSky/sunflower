using VirtueSky.Inspector;
using VirtueSky.Inspector.Elements;
using VirtueSky.Inspector.GroupDrawers;

[assembly: RegisterTriGroupDrawer(typeof(TriVerticalGroupDrawer))]

namespace VirtueSky.Inspector.GroupDrawers
{
    public class TriVerticalGroupDrawer : TriGroupDrawer<DeclareVerticalGroupAttribute>
    {
        public override TriPropertyCollectionBaseElement CreateElement(DeclareVerticalGroupAttribute attribute)
        {
            return new TriVerticalGroupElement();
        }
    }
}