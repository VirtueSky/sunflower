using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    /// <summary>
    /// Base class for every mediation client. Only holds what all mediations share.
    /// Ad formats are opt-in through capability interfaces (see AdClientCapabilities.cs),
    /// e.g. <see cref="IInterstitialClient"/>, <see cref="IAppOpenClient"/>, <see cref="INativeOverlayClient"/>.
    /// </summary>
    public abstract class AdClient
    {
        protected AdSetting adSetting;
        public bool SdkInitializationCompleted { get; internal set; }

        public abstract void Initialize();
        public abstract void ShowAdMediationDebugger();

        protected static void LoadAdUnit(AdUnitVariable unit)
        {
            if (unit == null || unit.IsShowing) return;
            if (!unit.IsReady() && !unit.IsLoading) unit.Load();
        }
    }
}
