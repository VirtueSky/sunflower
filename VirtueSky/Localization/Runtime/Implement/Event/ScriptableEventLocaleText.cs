using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "event_localetext.asset", menuName = "Pancake/Localization/Events/locale text")]
    [EditorIcon("scriptable_event")]
    public class ScriptableEventLocaleText : BaseEvent<LocaleText>
    {
    }
}