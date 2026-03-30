# Core scriptable object architecture for building Unity games (Android & iOS)

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
### 1: Download the repository and drop it into folder `Assets`
### 2: Add the line below to `Packages/manifest.json`

- for version `3.5.2`
```json
"com.virtuesky.sunflower":"https://github.com/VirtueSky/sunflower.git#3.5.2",
```
- depencies:
```json
"com.unity.nuget.newtonsoft-json": "3.2.1",
"com.unity.serialization": "3.1.1",
"com.unity.collections": "2.1.4",
"com.unity.textmeshpro": "3.0.8",
"com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask#2.5.10",
"com.kyrylokuzyk.primetween": "1.3.8",
```
## Includes modules

```bash
├── Core (Update only called once in Monobehaviour, Delay...)
├── Advertising (Support for Max, Admob and IronSource)
├── In App Purchase (IAP)
├── Asset Finder
├── Audio
├── Button
├── Data
├── Scriptable Event
├── Scriptable Variable
├── Firebase Remote Config
├── Tracking (Firebase Analytics, Adjust, AppsFlyer)
├── Tri-Inspector
├── Level Editor
├── Mobile Notification
├── Object Pooling
├── Prime tween
├── Localization
├── FolderIcons
├── Hierarchy
├── In app review
├── SimpleJSON
├── Tracking Revenue (by Firebase analytic, Adjust or Appsflyer)
├── Vibration (Vibration native support for android & ios)
├── Game Service (Sign in with apple id / google play games service)
├── Misc (Extension support Transform, SafeArea, Play Animancer, Skeleton,...)
├── Touch Input
├── Component
```

#### Note:

- [See Document](https://github.com/VirtueSky/sunflower/wiki)
- [Project implementation](https://github.com/VirtueSky/TheBeginning)
- [Core has similar modules but does not use scriptable architecture](https://github.com/wolf-package/unity-common)
