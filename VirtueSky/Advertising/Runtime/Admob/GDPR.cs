using System.Collections.Generic;
#if ADS_ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
#endif
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;

public class GDPR : MonoBehaviour
{
    [SerializeField] private bool isDontDestroyOnLoad;
    [SerializeField] private bool isTestDevice = false;

    [ShowIf(nameof(isTestDevice))] [SerializeField]
    private List<string> listTestDeviceHashedIds = new List<string>();

    [SerializeField] private BooleanVariable isCanRequestAds;

    private void Awake()
    {
        if (isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
#if ADS_ADMOB
        Init();
#endif
    }


#if ADS_ADMOB
    public void Init()
    {
#if !UNITY_EDITOR
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        string deviceIDUpperCase = deviceID.ToUpper();
        Debug.Log("TestDeviceHashedId = " + deviceIDUpperCase);
        if (isTestDevice)
        {
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings = new ConsentDebugSettings()
                {
                    DebugGeography = DebugGeography.EEA,
                    TestDeviceHashedIds = listTestDeviceHashedIds
                }
            };

            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }
        else
        {
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
            };

            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

#endif
    }

    private void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            Debug.Log("error consentError = " + consentError);
            return;
        }

        ConsentForm.LoadAndShowConsentFormIfRequired(
            (FormError formError) =>
            {
                if (formError != null)
                {
                    Debug.Log("error consentError = " + consentError);
                    return;
                }

                Debug.Log("ConsentStatus = " + ConsentInformation.ConsentStatus.ToString());
                Debug.Log("CanRequestAds = " + ConsentInformation.CanRequestAds());
                if (ConsentInformation.CanRequestAds())
                {
                    MobileAds.RaiseAdEventsOnUnityMainThread = true;
                    if (isCanRequestAds != null)
                    {
                        isCanRequestAds.Value = true;
                    }
                }
                else
                {
                    if (isCanRequestAds != null)
                    {
                        isCanRequestAds.Value = false;
                    }
                }
            }
        );
    }

    public bool GDPROnCanRequestAds => ConsentInformation.CanRequestAds();

    public void LoadAndShowConsentForm()
    {
        Debug.Log("LoadAndShowConsentForm Start!");

        ConsentForm.Load((consentForm, loadError) =>
        {
            if (loadError != null)
            {
                Debug.Log("error loadError = " + loadError);
                return;
            }


            consentForm.Show(showError =>
            {
                if (showError != null)
                {
                    Debug.Log("error showError = " + showError);
                    return;
                }
            });
        });
    }

    public void GDPRReset()
    {
//#if !UNITY_EDITOR
        Debug.Log("Reset Start!");
        ConsentInformation.Reset();
//#endif
    }
#endif
}