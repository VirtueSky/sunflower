using VirtueSky.Inspector.Processors;
using VirtueSky.Inspector;
using UnityEngine;

[assembly: RegisterTriPropertyHideProcessor(typeof(HideInPlayModeProcessor))]

namespace VirtueSky.Inspector.Processors
{
    public class HideInPlayModeProcessor : TriPropertyHideProcessor<HideInPlayModeAttribute>
    {
        public override bool IsHidden(TriProperty property)
        {
            return Application.isPlaying != Attribute.Inverse;
        }
    }
}