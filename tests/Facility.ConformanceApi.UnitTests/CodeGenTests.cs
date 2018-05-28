using System.IO;
using System.Linq;
using System.Reflection;
using Facility.CodeGen.CSharp;
using Facility.Definition;
using Facility.Definition.Fsd;
using FluentAssertions;
using NUnit.Framework;

namespace Facility.ConformanceApi.UnitTests
{
	public sealed class CodeGenTests
	{
		[Test]
		public void GenerateConformanceApi()
		{
			ServiceInfo service;
			const string fileName = "Facility.ConformanceApi.UnitTests.ConformanceApi.fsd";
			var parser = new FsdParser();
			using (var reader = new StreamReader(GetType().GetTypeInfo().Assembly.GetManifestResourceStream(fileName)))
				service = parser.ParseDefinition(new ServiceDefinitionText(Path.GetFileName(fileName), reader.ReadToEnd()));

			var generator = new CSharpGenerator { GeneratorName = "CodeGenTests" };
			var output = generator.GenerateOutput(service);
			output.Files.Count(x => x.Name == "IConformanceApi.g.cs").Should().Be(1);
		}
	}
}