using VirtueSky.Inspector;
using VirtueSky.Inspector.Elements;
using VirtueSky.Inspector.GroupDrawers;

[assembly: RegisterTriGroupDrawer(typeof(TriTabGroupDrawer))]

namespace VirtueSky.Inspector.GroupDrawers
{
    public class TriTabGroupDrawer : TriGroupDrawer<DeclareTabGroupAttribute>
    {
        public override TriPropertyCollectionBaseElement CreateElement(DeclareTabGroupAttribute attribute)
        {
            return new TriTabGroupElement();
        }
    }
}