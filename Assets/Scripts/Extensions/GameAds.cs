using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public static class GameAds {
    public const string testDevice_ID = "96a83f8abdc68f5a911e8fa694738814";
    public const string banner_main_ID = "ca-app-pub-5934552828632854/7482110921";
    public const string interstitial_onrestart_ID = "ca-app-pub-5934552828632854/3132636526";

    private static BannerView _banner;
    private static InterstitialAd _ad_interstitial;

    private static void Init() {
        _ad_interstitial = new InterstitialAd(GameAds.interstitial_onrestart_ID);
        AdRequest _interstitial_request = new AdRequest.Builder().Build(); //.AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice(GameAds.testDevice_ID).
        _ad_interstitial.LoadAd(_interstitial_request);

        _banner = new BannerView(GameAds.banner_main_ID, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build(); //.AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice(GameAds.testDevice_ID).Build();
        _banner.LoadAd(request);
        _banner.OnAdLoaded += OnBannerLoaded;
    }


    private static void OnBannerLoaded(object sender, System.EventArgs args) {
        _banner.Show();
    }

    private static void OnInterstitialLoaded(object sender, System.EventArgs args) {
        _ad_interstitial.Show();
    }

    public static void ShowInterstitial() {
        if (_ad_interstitial.IsLoaded()) {
            _ad_interstitial.Show();
        }
    }

    static GameAds() {
        Init();
    }
}
