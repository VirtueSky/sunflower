using VirtueSky.Inspector.Processors;
using VirtueSky.Inspector;
using UnityEngine;

[assembly: RegisterTriPropertyHideProcessor(typeof(HideInEditModeProcessor))]

namespace VirtueSky.Inspector.Processors
{
    public class HideInEditModeProcessor : TriPropertyHideProcessor<HideInEditModeAttribute>
    {
        public override bool IsHidden(TriProperty property)
        {
            return Application.isPlaying == Attribute.Inverse;
        }
    }
}