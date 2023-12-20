using VirtueSky.Inspector.Processors;
using VirtueSky.Inspector;
using UnityEngine;

[assembly: RegisterTriPropertyDisableProcessor(typeof(DisableInEditModeProcessor))]

namespace VirtueSky.Inspector.Processors
{
    public class DisableInEditModeProcessor : TriPropertyDisableProcessor<DisableInEditModeAttribute>
    {
        public override bool IsDisabled(TriProperty property)
        {
            return Application.isPlaying == Attribute.Inverse;
        }
    }
}