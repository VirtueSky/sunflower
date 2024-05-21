# Core ScriptableObject architecture for building Unity games (Android & Ios)

<p align="left">
  <a>
    <img alt="Made With Unity" src="https://img.shields.io/badge/made%20with-Unity-57b9d3.svg?logo=Unity">
  </a>
  <a>
    <img alt="License" src="https://img.shields.io/github/license/VirtueSky/sunflower?logo=github">
  </a>
  <a>
    <img alt="Last Commit" src="https://img.shields.io/github/last-commit/VirtueSky/sunflower?logo=Mapbox&color=orange">
  </a>
  <a>
    <img alt="Repo Size" src="https://img.shields.io/github/repo-size/VirtueSky/sunflower?logo=VirtualBox">
  </a>
  <a>
    <img alt="Last Release" src="https://img.shields.io/github/v/release/VirtueSky/sunflower?include_prereleases&logo=Dropbox&color=yellow">
  </a>
</p>

### Unity 2022.3 LTS
## How To Install

### Add the line below to `Packages/manifest.json`

for version `2.7.1`
```csharp
"com.virtuesky.sunflower":"https://github.com/VirtueSky/sunflower.git#2.7.1",
```

## Includes modules

```bash
├── Core (Update only called once in Monobehaviour,...)
├── ATT_IOS
├── Advertising (Support for Max & Admob)
├── In App Purchase (IAP)
├── Asset Finder
├── Audio
├── Button
├── Data
├── Scriptable Event
├── Scriptable Variable
├── Firebase (Remote config, Analytic)
├── Tri-Inspector
├── Level Editor
├── Mobile Notification
├── Object Pooling
├── Prime tween
├── Hierarchy
├── In app review
├── UniTask
├── SimpleJSON
├── Tracking Revenue (by Firebase analytic, Adjust or Appsflyer)
├── Vibration (Vibration native support for android & ios)
├── Game Service (Login by Google Play Game or Apple Auth)
├── Misc (Extension support Transform, SafeArea, Play Animancer, Skeleton,...)
├── Component
```

#### Note:

- [See Document](https://github.com/VirtueSky/sunflower/wiki)
- [Project implementation](https://github.com/VirtueSky/TheBeginning)
- [Core has similar modules that use singleton](https://github.com/wolf-package/unity-common)
- (Reference: https://github.com/pancake-llc/foundation/tree/main/Assets/Heart)
