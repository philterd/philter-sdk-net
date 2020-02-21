# Philter SDK for .NET

The **Philter SDK for .NET** enables .NET developers to easily work with Philter. [Philter](https://www.mtnfog.com/products/philter/) identifies and manipulates sensitive information like Protected Health Information (PHI) and personally identifiable information (PII) from natural language text. 

[![Build Status](https://travis-ci.org/mtnfog/philter-sdk-net.svg?branch=master)](https://travis-ci.org/mtnfog/philter-sdk-net)

## Usage

Clone and import the project into your solution.

```
git clone https://github.com/mtnfog/philter-sdk-net.git
```

To filter text:

```
PhilterClient client = new PhilterClient("https://127.0.0.1:8080");
string filteredText = client.Filter(text);
```

## License

This project is licensed under the Apache Software License, version 2.0.

Copyright 2020 Mountain Fog, Inc.
