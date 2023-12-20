using VirtueSky.Inspector.Processors;
using VirtueSky.Inspector;
using UnityEngine;

[assembly: RegisterTriPropertyDisableProcessor(typeof(DisableInPlayModeProcessor))]

namespace VirtueSky.Inspector.Processors
{
    public class DisableInPlayModeProcessor : TriPropertyDisableProcessor<DisableInPlayModeAttribute>
    {
        public override bool IsDisabled(TriProperty property)
        {
            return Application.isPlaying != Attribute.Inverse;
        }
    }
}