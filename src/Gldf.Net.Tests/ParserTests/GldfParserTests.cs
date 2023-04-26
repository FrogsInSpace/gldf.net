using FluentAssertions;
using Gldf.Net.Parser;
using Gldf.Net.Tests.TestData;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;

namespace Gldf.Net.Tests.ParserTests;

[TestFixture]
public class GldfParserTests
{
    private string _tempGldfPath;
    private static readonly TestCaseData[] ValidXmlTestCases = EmbeddedXmlTestData.ValidXmlTestCases;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _tempGldfPath = Path.GetTempFileName();
        var gldfBytes = EmbeddedGldfTestData.GetGldfWithFilesComplete();
        File.WriteAllBytes(_tempGldfPath, gldfBytes);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        File.Delete(_tempGldfPath);
    }

    [Test]
    public void ParseFromContainerFile_ShouldReturnNotNull()
    {
        using var client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(10) };
        var settings = new ParserSettings(LocalFileLoadBehaviour.Load, OnlineFileLoadBehaviour.Load, client);
        var gldfParser = new GldfParser(settings);

        var rootTyped = gldfParser.ParseFromGldfFile(_tempGldfPath);
        rootTyped.Should().NotBeNull();
    }

    [Test]
    public void ParseFromXml_ShouldReturnNotNull()
    {
        var gldf = EmbeddedXmlTestData.GetChangeableCompleteXml();
        using var client = new HttpClient();
        var settings = new ParserSettings(LocalFileLoadBehaviour.Load, OnlineFileLoadBehaviour.Load, client);
        var gldfParser = new GldfParser(settings);

        var rootTyped = gldfParser.ParseFromXml(gldf);
        rootTyped.Should().NotBeNull();
    }
    
    [Test, TestCaseSource(nameof(ValidXmlTestCases))]
    public void ParseFromXml_ShouldProcessAllXml(string xml)
    {
        var settings = new ParserSettings(LocalFileLoadBehaviour.Skip);
        var gldfParser = new GldfParser(settings);
        var rootTyped = gldfParser.ParseFromXml(xml);
        rootTyped.Should().NotBeNull();
    }
}