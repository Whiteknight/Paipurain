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

### Creating a simple pipeline
To create a pipeline you need to use the `PipelineBuilder` class:
```csharp

// Input and Output of the pipeline is string in this case
var builder = new PipelineBuilder<string>()
	.Begin((val) => $"{val}_initialBlock")
```

### Common mistakes

#### Don't using the Begin method
The `Begin()` method must be always used to start building the pipeline:
```csharp

// WRONG
var builder = new PipelineBuilder<string>()
	.AddBlock<string, string>(val => $"{val}_block1")
	.....

// RIGHT
var builder = new PipelineBuilder<string>()
	.Begin((val) => $"{val}_initialBlock")
	.AddBlock<string, string>(val => $"{val}_block1")
	...
```

## Contributing

__Contributions are always welcome!__  
Just send me a pull request and I will look at it. If you have more changes please create a issue to discuss it first.

## Donate
If you like my software, please consider [supporting me](https://paypal.me/alphadaniel) with a little donation. Thank you for your support! You are great!

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
