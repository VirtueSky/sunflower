namespace VirtueSky.Ads
{
    // Each interface describes one ad format. An AdClient implements only the formats its mediation supports.

    public interface IBannerClient
    {
        void LoadBanner();
    }

    public interface IInterstitialClient
    {
        void LoadInterstitial();
    }

    public interface IRewardClient
    {
        void LoadRewarded();
    }

    public interface IRewardedInterstitialClient
    {
        void LoadRewardedInterstitial();
    }

    public interface IAppOpenClient
    {
        void LoadAppOpen();
        void ShowAppOpen();
    }

    // Currently, native overlay ads is only available for admob.
    public interface INativeOverlayClient
    {
        void LoadNativeOverlay();
    }
}
