# VWO .NET SDK

[![NuGet](https://img.shields.io/nuget/v/VWO.Sdk.svg?style=plastic)](https://www.nuget.org/packages/VWO.Sdk/)
[![Build Status](http://img.shields.io/travis/wingify/vwo-dotnet-sdk/master.svg?style=flat)](http://travis-ci.org/wingify/vwo-dotnet-sdk)
[![Coverage Status](https://img.shields.io/coveralls/wingify/vwo-dotnet-sdk.svg)](https://coveralls.io/r/wingify/vwo-dotnet-sdk)

This open source library allows you to A/B Test your Website at server-side.

## Requirements

* Works with NetStandard: 2.0 onwards.

## Installation

```bash
PM> Install-Package VWO.Sdk
```

## Basic usage

**Using and Instantiation**

```c#
using VWOSdk;

Settings settingsFile = VWO.GetSettings(accountId, sdkKey);     //  Fetch settingsFile from VWO.
IVWOClient vwoClient = VWO.Instantiate(settingsFile);           //  Create VWO Client to user APIs.
```

**API usage**

```c#
// Activate API
string variationName = vwoClient.Activate(campaignTestKey, userId);

// GetVariation API
string variationName = vwoClient.GetVariation(campaignTestKey, userId);

// Track API
// For CUSTOM CONVERSION Goal
bool isSuccessful = vwoClient.Track(campaignTestKey, userId, goalIdentifier);

// For Revenue Goal
bool isSuccessful = vwoClient.Track(campaignTestKey, userId, goalIdentifier, revenueValue);
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

**User Profile Service**

```c#
using VWOSdk;

public class UserProfileService : IUserProfileService
{
    public UserProfileMap Lookup(string userId)
    {
        // ...code here for getting data
        // return data
    }

    public void Save(UserProfileMap userProfileMap)
    {
        // ...code to persist data
    }
}


var settingsFile = VWO.GetSettings(VWOConfig.AccountId, VWOConfig.SdkKey);

//  Provide UserProfileService instance while vwoClient Instantiation.
var vwoClient = VWO.Instantiate(settingsFile, userProfileService: new UserProfileService());
```

## Documentation

Refer [Official VWO Documentation](https://developers.vwo.com/reference#server-side-introduction)

## Demo NetStandard application

[vwo-dotnet-sdk-example](https://github.com/wingify/vwo-dotnet-sdk-example)

## Setting Up development environment

```bash
chmod +x start-dev.sh; ./start-dev;
```

It will install the git-hooks necessary for commiting and pushing the code. Commit-messages follow a [guideline](https://github.com/angular/angular/blob/master/CONTRIBUTING.md#-commit-message-guidelines). All test cases must pass before pushing the code.

## Running Unit Tests

```bash
dotnet test
```

## Credits

We use the following open-source projects. Thanks to the authors and maintainers of the corresponding projects.

Projects which are published under MIT License:
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) by [@JamesNK](https://github.com/JamesNK)

Projects which are published under Apache License 2.0 License:
* [murmurhash](https://github.com/darrenkopp/murmurhash-net) by [@darrenkopp](https://github.com/darrenkopp)
* [Identifiable](https://github.com/seanterry/Identifiable) by [@seanterry](https://github.com/seanterry)

## Authors

* Main Contributor - [Sidhant Gakhar](https://github.com/sidhantgakhar)
* Repo health maintainer - [Varun Malhotra](https://github.com/softvar)([@s0ftvar](https://twitter.com/s0ftvar))

## Contributing

Please go through our [contributing guidelines](https://github.com/wingify/vwo-dotnet-sdk/CONTRIBUTING.md)

## Code of Conduct

[Code of Conduct](https://github.com/wingify/vwo-dotnet-sdk/blob/master/CODE_OF_CONDUCT.md)

## License

```text
    MIT License

    Copyright (c) 2019 Wingify Software Pvt. Ltd.

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
```
