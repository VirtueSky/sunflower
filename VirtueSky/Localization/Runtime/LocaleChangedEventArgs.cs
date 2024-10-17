using System;

namespace VirtueSky.Localization
{
    public class LocaleChangedEventArgs : EventArgs
    {
        public LocaleChangedEventArgs(Language previous, Language current)
        {
            Previous = previous;
            Current = current;
        }

        public Language Previous { get; private set; }
        public Language Current { get; private set; }
    }
}