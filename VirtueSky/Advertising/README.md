## Advertising for unity
(Supports applovin 5.11.3 and google admob 8.5.2)
- built on scriptableobject architecture, you need to install [Sunflower](https://github.com/VirtueSky/sunflower) to use

# Use
- After installation, open Adsetting to set up AdUnit Id
  
    (Ctrl + E or GameControl -> adSetting to open Adsetting)

    ![OpenAdsetting](https://github.com/wolf-package/advertising/assets/126542083/fc1d053d-56cc-4126-a6db-6cf03a2a6b46)
- You can choose Ad Network Applovin for Admob and click `Create` button to create AdUnitiVariable
  
    ![Adsetting](https://github.com/wolf-package/advertising/assets/126542083/d5026c44-61bd-4c29-a6de-e2d84003d24e)

- Set id for AdUnitVariable

    ![AdVariable](https://github.com/wolf-package/advertising/assets/126542083/f351e91d-9542-424b-89ff-25f6afa7ccec)

    (For Admob, you can get the test Id by going to `Context Menu` and selecting `Get Id Test`)
  
    ![idtest](https://github.com/wolf-package/advertising/assets/126542083/7ec8241b-5449-478d-be61-40eb38e53334)

- Attach `Advertising` to the object in the scene (loading scene) to load the ad
  
    ![Adsvertising](https://github.com/wolf-package/advertising/assets/126542083/47b53d5a-a65f-41be-b2f9-8e06cd09cb77)

- Import `Scripting Define Symbols` in `Project Settings` > `Player` > `Other Settings`
    -   For Applovin: `VIRTUESKY_ADS` and `ADS_APPLOVIN`
    -   For Admob: `VIRTUESKY_ADS` and `ADS_ADMOB`
      
    ![Screenshot 2023-10-03 175544](https://github.com/wolf-package/advertising/assets/56286032/44ab8706-b06e-49da-86fa-c67663ed4339)


- Script show ads (demo)
  
```
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VirtueSky.Ads;

public class AdsManager : MonoBehaviour
{
    public TextMeshProUGUI textNoti;
    public AdUnitVariable banner;
    public AdUnitVariable inter;
    public AdUnitVariable reward;

    public void ShowBanner()
    {
        Debug.Log(banner.IsReady());
        banner.Show();
    }

    public void ShowInter()
    {
        if (inter.IsReady())
        {
            LogMessage("inter is ready");
            inter.Show().OnCompleted(() => { LogMessage("inter is completed"); });
        }
        else
        {
            LogMessage("Inter is not ready");
        }
    }

    public void ShowReward()
    {
        if (reward.IsReady())
        {
            LogMessage("reward is ready");
            reward.Show().OnCompleted(() => { LogMessage("Reward is completed"); }).OnSkipped(() => { LogMessage("reward is skipped"); });
        }
        else
        {
            LogMessage("reward is not ready");
        }
    }

    void LogMessage(string message)
    {
        textNoti.text = message;
        Debug.Log(message);
    }
}
```
