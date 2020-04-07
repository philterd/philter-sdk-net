# Philter SDK for .NET

The **Philter SDK for .NET** enables .NET developers to easily work with Philter. [Philter](https://www.mtnfog.com/products/philter/) identifies and manipulates sensitive information like Protected Health Information (PHI) and personally identifiable information (PII) from natural language text.

Refer to [Philter API](https://philter.mtnfog.com/api/) documentation for details on the methods available.

[![Build Status](https://travis-ci.org/mtnfog/philter-sdk-net.svg?branch=master)](https://travis-ci.org/mtnfog/philter-sdk-net)

## Build

The project can be built using Visual Studio or other .NET IDE. It can be built via the command line using either the `build.bat` or `build.sh` scripts depending on your platform.

## Usage

Install using [NuGet](https://www.nuget.org/packages/philter-sdk-net/).

[![nuget](https://img.shields.io/nuget/v/philter-sdk-net.svg)](https://www.nuget.org/packages/philter-sdk-net/)

Or, clone and import the project into your solution.

```
git clone https://github.com/mtnfog/philter-sdk-net.git
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

* 1.1.0 - Added authentication support.
* 1.0.0 - Initial release.

## License

This project is licensed under the Apache Software License, version 2.0.

Copyright 2020 Mountain Fog, Inc.
