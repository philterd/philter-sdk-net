# Philter SDK for .NET

The Philter SDK for .NET is a client for de-identifying and redacting text using Philter. Refer to the [Philter API](https://docs.philterd.ai/philter/api/) documentation for details on the methods available.

## Build

The project can be built using Visual Studio or other .NET IDE. It can be built via the command line using either the `build.bat` or `build.sh` scripts depending on your platform.

## Usage

Install using [NuGet](https://www.nuget.org/packages/philter-sdk-net/).

Or, clone and import the project into your solution.

```
git clone https://github.com/philterd/philter-sdk-net.git
```

With an available running instance of Philter, to filter text:

```
PhilterClient client = new PhilterClient("https://127.0.0.1:8080");
string filteredText = client.Filter(text);
```

To filter text with explanation:

```
PhilterClient client = new PhilterClient("https://127.0.0.1:8080");
ExplainResponse explainResponse = client.Explain(text);
```

## Release History

* 1.3.1 - Updates for RestSharp 110.2.0.
* 1.3.0 - Renamed filter profiles to policies. Updated to .NET 7.0.
* 1.1.0 - Added authentication support.
* 1.0.0 - Initial release.

## License

This project is licensed under the Apache License, version 2.0.

Copyright 2023 Philterd, LLC
Philter is a registered trademark of Mountain Fog, Inc.
