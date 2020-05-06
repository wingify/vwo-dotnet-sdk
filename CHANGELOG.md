# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.5.2] - 2020-04-30

### Changed

- variationTargetingVariables argument support added in APIs: `activate`, `getVariation`, `track`, `isFeatureEnabled`, and `getFeatureVariableValue`.

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
            {"_vwoUserId", "User"}
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
