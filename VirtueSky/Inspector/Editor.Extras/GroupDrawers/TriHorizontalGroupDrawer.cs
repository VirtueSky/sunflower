using VirtueSky.Inspector;
using VirtueSky.Inspector.Elements;
using VirtueSky.Inspector.GroupDrawers;
using UnityEngine;

[assembly: RegisterTriGroupDrawer(typeof(TriHorizontalGroupDrawer))]

namespace VirtueSky.Inspector.GroupDrawers
{
    public class TriHorizontalGroupDrawer : TriGroupDrawer<DeclareHorizontalGroupAttribute>
    {
        public override TriPropertyCollectionBaseElement CreateElement(DeclareHorizontalGroupAttribute attribute)
        {
            return new TriHorizontalGroupElement(attribute.Sizes);
        }
    }
}