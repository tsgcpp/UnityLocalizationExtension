# Feature

## StringTableCollectionBundle
This feature is useful if you want to pull multiple "StringTableCollection"s at once.

- `Right click -> Create -> Tsgcpp -> Localization Extension -> StringTableCollectionBundle`
- Select `StringTableCollectionBundle` (ScriptableObject)
- Set `SheetsServiceProvider`
- Set folders that contain "StringTableCollection"s
- Click `Pull` or `Push` for the synchronization

![image](https://user-images.githubusercontent.com/19503967/181578051-2cac62ba-a1d8-47fa-8ea5-291c43e929a2.jpg)


## ServiceAccountSheetsServiceProvider (for Google Sheets)
This feature is useful if you want to use GCP Service Account to access Google Sheets.
**Private** Google Sheets can be accessed by using GCP Service Account.
Here is the example code.

```cs
    var bundle = AssetDatabase.LoadAssetAtPath<StringTableCollectionBundle>(path);

    var provider = new ServiceAccountSheetsServiceProvider(
        serviceAccountKeyJson: "<GCP Service Account Key (Json format)>",
        applicationName: bundle.SheetsServiceProvider.ApplicationName);

    bundle.PullAllLocales(provider);
```

See an example [ExampleLocalizationSynchronizationMenu.cs](./Assets/Example/Editor/ExampleLocalizationSynchronizationMenu.cs).

### Appendix: Service Account Setting
- Create GCP service account

![image](https://user-images.githubusercontent.com/19503967/181583555-3337bbe1-c95d-4703-95a8-655660ae8d8e.jpg)

- Share the sheets with the service account
  - Viewer: Only Pull
  - Editor: Pull and Push

![image](https://user-images.githubusercontent.com/19503967/181583551-cc4f2839-cdd5-4e2d-b6af-400d2f839db4.jpg)
![image](https://user-images.githubusercontent.com/19503967/181583542-3579d781-1f18-459a-9d37-a68dc8f640b5.jpg)
