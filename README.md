# VWO .NET SDK

[![NuGet](https://img.shields.io/nuget/v/VWO.Sdk.svg?style=plastic)](https://www.nuget.org/packages/VWO.Sdk/)
[![Build Status](http://img.shields.io/travis/wingify/vwo-dotnet-sdk/master.svg?style=flat)](http://travis-ci.org/wingify/vwo-dotnet-sdk)
[![Coverage Status](https://img.shields.io/coveralls/wingify/vwo-dotnet-sdk.svg)](https://coveralls.io/r/wingify/vwo-dotnet-sdk)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](http://www.apache.org/licenses/LICENSE-2.0)

This open source library allows you to A/B Test your Website at server-side.

## Requirements

- Works with NetStandard: 2.0 onwards.

## Installation

```bash
PM> Install-Package VWO.Sdk
```

## Basic usage

**Using and Instantiation**

```c#
using VWOSdk;

Settings settingsFile = VWO.GetSettings(accountId, sdkKey);     //  Fetch settingsFile from VWO.
IVWOClient vwoClient = VWO.Launch(settingsFile);           //  Create VWO Client to user APIs.
```

**API usage**

```c#
using System.Collections.Generic;

// Activate API
// Without Custom Variable
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>(){};
string variationName = vwoClient.Activate(campaignKey, userId, options);

// With Custom Variable , variation targeting variable
public readonly static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
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

string variationName = vwoClient.Activate(campaignKey, userId, options);

// GetVariationName API
// Without Custom Variable
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>(){};
string variationName = vwoClient.GetVariationName(campaignKey, userId, options);

// With Custom Variable snd variation targeting variable
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    {
        "customVariables", new Dictionary<string, dynamic>()
        {
            {"price", '40'}
        }
        "variationTargetingVariables", new Dictionary<string, dynamic>()
        {
            {"team", "qa-internal"}
        }
    }
};
string variationName = vwoClient.GetVariationName(campaignKey, userId, options);

// Track API
// For CUSTOM CONVERSION Goal
bool isSuccessful = vwoClient.Track(campaignKey, userId, goalIdentifier);

// For Goal Conversion in Multiple Campaign
Dictionary<string, bool> result = vwoClient.Track(List <string>() { campaignKey1 campaignKey2 }, userId, goalIdentifier);

// For Goal Conversion in All Possible Campaigns
Dictionary<string, bool> result = vwoClient.Track(userId, goalIdentifier);

// Without Revenue Value and Custom Variable
Dictionary<string, dynamic> options = new Dictionary<string, dynamic>(){};
bool isSuccessful = vwoClient.Track(campaignKey, userId, goalIdentifier, options);

// For only Revenue Value
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    { "revenueValue", 10.2 },
};
bool isSuccessful = vwoClient.Track(campaignKey, userId, goalIdentifier, options);

// For only Custom Variable
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    {
        "customVariables", new Dictionary<string, dynamic>()
        {
            {"location", 'India'}
        }
    }
};
bool isSuccessful = vwoClient.Track(campaignKey, userId, goalIdentifier, options);

// For Revenue Value and Custom Variable and Variation Targeting varaible
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    {
        "revenue_value", 10
    },
    {
      "customVariables", new Dictionary<string, dynamic>()
      {
          {
            "gender", "f"
          }
      }
    },
    {
      "variationTargetingVariables", new Dictionary<string, dynamic>()
      {
          {
              "abcd", 1
          }
      }
    }
};
bool isSuccessful = vwoClient.Track(campaignKey, userId, goalIdentifier, options);

//IsFeatureEnabled API
//Without Custom Variable
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>(){};
bool isSuccessful = vwo.Client.IsFeatureEnabled(campaignKey, userId, options);

//With Custom Variable
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    {
        "customVariables", new Dictionary<string, dynamic>()
        {
            {"value", 10}
        }
    }
};
bool isSuccessful = vwo.Client.IsFeatureEnabled(campaignKey, userId, options);

//GetFeatureVariableValue API
//Without Custom Variable
Dictionary<string, dynamic> options = new Dictionary<string, dynamic>(){};
dynamic variableValue = vwo.Client.GetFeatureVariableValue(campaignKey, variableKey, userId, options);

//With Custom Variable
public static Dictionary<string, dynamic> options = new Dictionary<string, dynamic>()
{
    {
        "customVariables", new Dictionary<string, dynamic>()
        {
            {"value", 10}
        }
    }
};
dynamic variableValue = vwo.Client.GetFeatureVariableValue(campaignKey, variableKey, userId, options);

//Push API
bool isSuccessful = vwo.Client.Push(tagKey, tagValue, userId);

//Pass TagKey
var TagKey = "abc";
bool isSuccessful = vwo.Client.Push(TagKey, tagValue, userId);

//Pass TagValue
var TagValue = "abc";
bool isSuccessful = vwo.Client.Push(tagKey, TagValue, userId);
```

**Configure Log Level**

```c#
VWO.Configure(LogLevel.DEBUG);
```

**Implement and Configure Custom Logger** - implement your own logger class

```c#
using VWOSdk;

public class CustomLogWriter : ILogWriter
{
    public void WriteLog(LogLevel logLevel, string message)
    {
        // ...write to file or database or integrate with any third-party service
    }
}

//  Configure Custom Logger with SDK.
VWO.Configure(new CustomLogWriter());
```

**User Storage Service**

```c#
using VWOSdk;

public class UserStorageService : IUserStorageService
{
    public UserStorageMap Get(string userId)
    {
        // ...code here for getting data
        // return data
    }

    public void Set(UserStorageMap userStorageMap)
    {
        // ...code to persist data
    }
}


var settingsFile = VWO.GetSettings(VWOConfig.AccountId, VWOConfig.SdkKey);

//  Provide UserStorageService instance while vwoClient Instantiation.
var vwoClient = VWO.Launch(settingsFile, userStorageService: new UserStorageService());

//  Set specific goalType to Track
//  Available GoalTypes - GoalTypes.REVENUE, GoalTypes.CUSTOM, GoalTypes.ALL (Default)
var vwoClient = VWO.Launch(settingsFile, goalTypeToTrack: Constants.GoalTypes.REVENUE);

//  Set if a return user should be tracked, default false
var vwoClient = VWO.Launch(settingsFile, shouldTrackReturningUser: true);
```

## Documentation

Refer [Official VWO Documentation](https://developers.vwo.com/reference#fullstack-introduction)

## Demo NetStandard application

[vwo-dotnet-sdk-example](https://github.com/wingify/vwo-dotnet-sdk-example)

## Setting Up development environment

```bash
chmod +x start-dev.sh;
bash start-dev.sh;
```

It will install the git-hooks necessary for commiting and pushing the code. Commit-messages follow a [guideline](https://github.com/angular/angular/blob/master/CONTRIBUTING.md#-commit-message-guidelines). All test cases must pass before pushing the code.

## Running Unit Tests

```bash
dotnet test
```

## Third-party Resources and Credits

Refer [third-party-attributions.txt](https://github.com/wingify/vwo-dotnet-sdk/blob/master/third-party-attributions.txt)

## Authors

- Main Contributor - [Sidhant Gakhar](https://github.com/sidhantgakhar)
- Repo health maintainer - [Varun Malhotra](https://github.com/softvar)([@s0ftvar](https://twitter.com/s0ftvar))

## Changelog

Refer [CHANGELOG.md](https://github.com/wingify/vwo-dotnet-sdk/blob/master/CHANGELOG.md)

## Contributing

Please go through our [contributing guidelines](https://github.com/wingify/vwo-dotnet-sdk/CONTRIBUTING.md)

## Code of Conduct

[Code of Conduct](https://github.com/wingify/vwo-dotnet-sdk/blob/master/CODE_OF_CONDUCT.md)

## License

[Apache License, Version 2.0](https://github.com/wingify/vwo-dotnet-sdk/blob/master/LICENSE)

Copyright 2019-2020 Wingify Software Pvt. Ltd.
