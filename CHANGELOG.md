# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.27.2] - 2022-12-20

### Fixed

- fixed track revenue-goal issue, where `revenueValue` was being sent as null.

## [1.27.1] - 2022-08-31

### Changed

- Downgrade `Newtonsoft.Json` version from v13.0.1 to v12.0.2

## [1.27.0] - 2022-08-20

### Changed

- Update Newtonsoft.Json version from v11 to v13

## [1.26.0] - 2022-07-05

### Changed

- A tracking call wto VWO server was not being made when the feature was not enabled for the campaign's variation in case of `Feature Test` campaign. This was a bug and is fixed now.
- Fixes a bug when User Storage Service is used and `isFeatureEnabled` API is called for the returning visitor, it was always returning `false`.
- Add missing log for returning visitor in case of `isFeatureEnabled` API

## [1.25.0] - 2021-11-17

### Added

- Support for pushing multiple custom dimensions at once.
  Earlier, you had to call `push` API multiple times for tracking multiple custom dimensions as follows:

  ```c#
  bool isSuccessful = vwoClient.Push("browser", "chrome", userId);
  bool isSuccessful = vwoClient.Push("price", "20", userId);
  ```

  Now, you can pass a dictionary

  ```c#
  public static Dictionary<string, dynamic> customDimensionMap = new Dictionary<string, dynamic>()
    {
        {
            "browser", "chrome"
        },
        {
            "price", "10"
        }

    };

  bool isSuccessful = vwoClient.Push(customDimensionMap, userId);
  ```

  Multiple asynchronous tracking calls would be initiated in this case.

- Pass meta information from APIs to the User Storage Service's `set` method.

    ```c#
    Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
    {{
        "metaData", new Dictionary<string, dynamic>()
        {
            {
                "extrainfo", "some data"
            }
        }
    }};

    var variationName = vwoClient.Activate(campaignKey, userId, Options);
    ```

    Same for other APIs - `GetVariationName`, `Track`, `IsFeatureEnabled`, and `GetFeatureVariableValue`.

### Changed

- If Events Architecture is enabled for your VWO account, all the tracking calls being initiated from SDK would now be `POST` instead of `GET` and there would be single endpoint i.e. `/events/t`. This is done in order to bring events support and building advancded capabilities in future.

- For events architecture accounts, tracking same goal across multiple campaigns will not send multiple tracking calls. Instead one single `POST` call would be made to track the same goal across multiple different campaigns running on the same environment.

- Multiple custome dimension can be pushed via `push` API. For events architecture enabled account, only one single asynchronous call would be made to track multiple custom dimensions.

  ```c#
  public static Dictionary<string, dynamic> customDimensionMap = new Dictionary<string, dynamic>()
    {
        {
            "browser", "chrome"
        },
        {
            "price", "10"
        }

    };

  bool isSuccessful = vwoClient.Push(customDimensionMap, userId);
  ```

## [1.23.0] - 2020-10-20

### Added

- Introducing support for Mutually Exclusive Campaigns. By creating Mutually Exclusive Groups in VWO Application, you can group multiple FullStack A/B campaigns together that are mutually exclusive. SDK will ensure that visitors do not overlap in multiple running mutually exclusive campaigns and the same visitor does not see the unrelated campaign variations. This eliminates the interaction effects that multiple campaigns could have with each other. You simply need to configure the group in the VWO application and the SDK will take care what to be shown to the visitor when you will call the `activate` API for a given user and a campaign.

### Changed

- Remove `shouldTrackReturningVisitor` option as FullStack campaigns show unique visitors and conversions count. Duplicate visitors/conversions tracking calls would not be made if User Storage Service is used.

## [1.22.0] - 2020-09-28

### Added

- Feature Rollout and Feature Test campaigns now supports JSON type variable which can be created inside VWO Application. This will help in storing grouped and structured data.

- Use Campaign ID along with User ID for bucketing a user in a campaign. This will ensure that a particular user gets different variation for different campaigns having similar settings i.e. same campaign-traffic, number of variations, and variation traffic.

- Sending stats which are used for launching the SDK like storage service, logger, and integrations, etc. in tracking calls(track-user and batch-event). This is solely for debugging purpose. We are only sending whether a particular key(feature) is used not the actual value of the key

### Changed

- Sending visitor tracking call for Feature Rollout campaign when `isFeatureEnabled` API is used. This will help in visualizing the overall traffic for the respective campaign's report in the VWO application.

- Removed sending user-id, that is provided in the various APIs, in the tracking calls to VWO server as it might contain sensitive PII data.

- SDK Key will not be logged in any log message, for example, tracking call logs.


## [1.17.0] - 2020-06-22

### Changed

- `campaignName` will be available in integrations callback, if callback is defined.

## [1.14.0] - 2020-05-26

### Added

- Expose lifecycle hook events. This feature allows sending VWO data to third party integrations.

```csharp
internal class HookCallback : IntegrationEventListener
{
    public void onEvent(Dictionary<string, dynamic> properties)
    {
        string payLoad = JsonConvert.SerializeObject(properties);
        CustomLogger logger = new CustomLogger();
        logger.WriteLog(LogLevel.DEBUG, "onEvent call from SDK: " + payLoad);

    }
}

VWO.Launch(SettingsFile, integrations: new HookManager() { HookCallback = new HookCallback() });
```

### Changed

- Send environment token in every network call initiated from SDK to the VWO server. This will help in viewing campaign reports on the basis of environment.

- If User Storage Service is provided, do not track same visitor multiple times. You can pass `shouldTrackReturningUser` as true in case you prefer to track duplicate visitors.

```csharp
VWOClient = VWO.Launch(SettingsFile, userStorageService: new UserStorageService(),
                shouldTrackReturningUser: true);
```

- Introduce `integrations` param in `Launch` API to enable receiving hooks for the third party integrations.

## [1.11.0] - 2020-05-07

### Added

- Webhooks support
  - New API `GetAndUpdateSettingsFile` to fetch and update settings-file in case of webhook-trigger

- Added support for batching of events sent to VWO server
  - Intoduced `batchData` config in launch API for setting when to send batched events
  - Added `FlushEvents` API to manually flush the batch events queue whne batchEvents config is passed. Note: batchData config i.e. `EventsPerRequest` and `RequestTimeInterval` won't be considered while manually flushing

```csharp

Settings settingsFile = VWO.GetSettingsFile(accountId, sdkKey);

BatchEventData batchData = new BatchEventData();

batchData.EventsPerRequest = 100;
batchData.RequestTimeInterval = 86400;
batchData.FlushCallback = new FlushCallback();

IVWOClient vwoClientInstance = VWO.Launch(settingsFile, batchData = batchData);
```

### Changed

- `userStorageData` key can be passed in options parameter for utilizing already fetched storage data. It also helps in implementing the asynchronous nature of the User Storage Service's get method. For more info read [this](https://developers.vwo.com/reference#fullstack-is-user-storage-service-synchronous-or-asynchronous).

## [1.8.0] - 2020-07-28

### Changed

- Update track API to handle duplicate and unique conversions and corresponding changes in `Launch` API
- Update track API to track a goal globally across campaigns with the same `goalIdentififer` and corresponding changes in `Launch` API

```c#
// it will track goal having `goalIdentifier` of campaign having `campaignKey` for the user having `userId` as id.
vwoClientInstance.Track(campaignKey, userId, goalIdentifier, options);
// it will track goal having `goalIdentifier` of campaigns having `campaignKey1` and `campaignKey2` for the user having `userId` as id.
vwoClientInstance.Track(new List<string>() { campaignKey1, campaignKey2 }, userId, goalIdentifier, options);
// it will track goal having `goalIdentifier` of all the campaigns
vwoClientInstance.Track(userId, goalIdentifier, options);
//Read more about configuration and usage - https://developers.vwo.com/reference#server-side-sdk-track
```

## [1.6.1] - 2020-07-23

### Changed

- When there is no campaign running, `GetSettingsFileFile` API does not output correct result. This is now handled by validating it.

## [1.6.0] - 2020-05-07

### Added
Forced Variation capabilites
- Introduced `Forced Variation` to force certain users into specific variation. Forcing can be based on User IDs or custom variables defined.
### Changed
- All existing APIs to handle custom-targeting-variables as an option for forcing variation
- Code refactored to support Whitelisting.

## [1.5.2] - 2020-04-30

### Changed
- variationTargetingVariables argument support added in APIs: `activate`, `getVariationName`, `track`, `isFeatureEnabled`, and `getFeatureVariableValue`.

#### Before

```csharp
Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    {
        "custom_variables": new Dictionary<string, dynamic>()
        {
            {"value", 10}
        }
    }
};
```

```csharp
Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    {
        "customVariables", new Dictionary<string, dynamic>()
        {
            {"price", 100.1}
        },
        "variationTargetingVariables", new Dictionary<string, dynamic>()
        {
            {"team", "qa-internal"}
        }
    }
};
```

## [1.5.1] - 2020-04-13

### Breaking Changes

- `CreateInstance` API is renamed to `Launch` API.
- `custom_variables` key inside options is renamed to `customVariables`
- `revenue_value` key inside options is renamed to `revenueValue`

### Changed

- `GetVariation` can be used as an alternative for `GetVariationName` for backward compatibility.
- `GetVariationName` API does not send any impression

## [1.5.0] - 2020-02-20

### Breaking Changes

To prevent ordered arguments and increasing use-cases, we are moving all optional arguments to be passed via `options`.

- customVariables argument in APIs: `Activate`, `GetVariation`, `Track`, `IsFeatureEnabled`, and `GetFeatureVariableValue` via `options` argument.
- `revenueValue` parameter in `track` API in `options` argument

#### Before

```csharp
//  Activae API
string variationName = vwoClientInstance.Activate(campaignKey, userId);
// GetVariation API
string variationName = vwoClientInstance.GetVariation(campaignKey, userId);
// Track API
bool isSuccessful = vwoClientInstance.Track(campaignKey, userId, goalIdentifier, revenueValue);
```

#### After

```csharp
// Activate API
// With Custom Variables
Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    {
        "custom_variables": new Dictionary<string, dynamic>()
        {
            {"value", 10}
        }
    }
};
// Without Custom Variables
Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{};

// Activate API
string variationName = vwoClientInstance.Activate(campaignKey, userId, options);

// GetVariationName API
string variationName = vwoClientInstance.GetVariationName(campaignKey, userId, options);
// for backward compatibility
string variationName = vwoClientInstance.GetVariation(campaignKey, userId, options);

// Track API
bool isSuccessful = vwoClientInstance.Track(campaignKey, userId, goalIdentifier, options)

// With Revenue Value
Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    { "revenue_value", 10.2 },
};
bool isSuccessful = vwoClient.Track(campaignKey, userId, goalIdentifier, options);
```

### Added

- Feature Rollout and Feature Test capabilities
- Pre and Post segmentation capabilites
  Introduced new Segmentation service to evaluate whether user is eligible for campaign based on campaign pre-segmentation conditions and passed custom-variables

### Changed

- Existing APIs to handle new type of campaigns i.e. feature-rollout and feature-test
- All existing APIs to handle custom-variables for tageting audience
- Code refactored to support feature-rollout, feature-test, campaign tageting and post segmentation

## [1.3.0] - 2019-11-26

### Changed

- Change MIT License to Apache-2.0
- Added apache copyright-header in each file
- Add NOTICE.txt file complying with Apache LICENSE
- Give attribution to the third-party libraries being used and mention StackOverflow

## [1.1.1] - 2019-11-16

### Changed

- Downgrade dependency i.e. `Newtonsoft.Json` to `11.0.2` so it won;t conflict with other user-defined deps having this as dep.

## [1.0.1] - 2019-08-27

### Changed

- Test cases integration via `.travis.yml` and coveralls.

## [1.0.0] - 2019-08-23

### Added

- First beta release with Server-side A/B capabilities
