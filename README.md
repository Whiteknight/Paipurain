<h1 align="center">Paipurain</h1>
<div align="center">

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/built-with-grammas-recipe.svg)](https://forthebadge.com)

[![GitHub license](https://img.shields.io/github/license/LegendaryB/Paipurain.svg?longCache=true&style=flat-square)](https://github.com/LegendaryB/Paipurain/blob/master/LICENSE.md)
[![Nuget](https://img.shields.io/nuget/v/Paipurain.svg?style=flat-square)](https://www.nuget.org/packages/Paipurain/)
[![Donate](https://img.shields.io/badge/Donate-PayPal-blue.svg)](https://paypal.me/alphadaniel)

Simple and easy to use .NET Standard pipeline pattern implementation.

<sub>Built with ❤︎ by Daniel Belz</sub>
</div><br>

## Getting started

### Creating a pipeline with same input and output type
```csharp
// Build it
var builder = new PipelineBuilder<bool>()
	.AddBlock<bool, string>((val) => $"Input value was: {val}")
	.AddBlock<string, bool>((val) => string.IsNullOrWhiteSpace(val));

// Create the pipeline
var pipeline = builder.Build();

// PROCESS IT
await pipeline.Process(false);	
```

### Creating a pipeline with different input and output type
```csharp
// Build it
var builder = new PipelineBuilder<string, bool>()
	.AddBlock<string, bool>((val) => string.IsNotNullOrWhitespace(val));

// Create the pipeline
var pipeline = builder.Build();

// PROCESS IT
await pipeline.Process("Not empty string"); // returns true
await pipeline.Process(" "); // returns false
await pipeline.Process(""); // returns false	
```

### Common mistakes

#### Blocks not matching input type
```csharp
// Build it
var builder = new PipelineBuilder<string, bool>()
	.AddBlock<string, bool>(...)
	.AddBlock<string, bool>(...); // <--- WRONG (input must be bool because of previous block)

// Create the pipeline
var pipeline = builder.Build(); // throws RuntimeBinderException
```

#### First block not matching input type
```csharp
// Build it
var builder = new PipelineBuilder<string, bool>()
	.AddBlock<bool, bool>(...);

// Create the pipeline
var pipeline = builder.Build(); // throws RuntimeBinderException
```

#### Last block not matching output type
```csharp
// Build it
var builder = new PipelineBuilder<string, bool>()
	.AddBlock<string, string>(...);

// Create the pipeline
var pipeline = builder.Build(); // throws RuntimeBinderException
```

## Contributing

__Contributions are always welcome!__  
Just send me a pull request and I will look at it. If you have more changes please create a issue to discuss it first.

## Donate
If you like my software, please consider [supporting me](https://paypal.me/alphadaniel) with a little donation. Thank you for your support! You are great!

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
