﻿using FluentAssertions;
using Gldf.Net.Domain.Head.Types;
using Gldf.Net.Exceptions;
using Gldf.Net.XmlHelper;
using NUnit.Framework;
using System;
using System.Xml;

namespace Gldf.Net.Tests.XmlHelperTests
{
    [TestFixture]
    public class XmlFormatVersionReaderTests
    {
        [Test]
        public void GetFormatVersion_Should_BeExpected()
        {
            const string version = "0.9";
            const FormatVersion expected = FormatVersion.V09;
            var xml = $"<Root><Header><FormatVersion>{version}</FormatVersion></Header></Root>";

            var formatVersion = GldfFormatVersionReader.GetFormatVersion(xml);

            formatVersion.Should().Be(expected);
        }

        [Test]
        public void GetFormatVersion_Should_Throw_When_InvalidXml()
        {
            const string xml = "notValidXml";

            Action act = () => GldfFormatVersionReader.GetFormatVersion(xml);

            act.Should()
                .ThrowExactly<GldfException>()
                .WithMessage("Failed to get FormatVersion XML element")
                .WithInnerException<XmlException>()
                .WithMessage("Data at the root level is invalid*");
        }

        [Test]
        public void GetFormatVersion_Should_Throw_When_MissingFormatElement()
        {
            const string xml = "<Root><Header></Header></Root>";

            Action act = () => GldfFormatVersionReader.GetFormatVersion(xml);

            act.Should()
                .ThrowExactly<GldfException>()
                .WithMessage("Failed to get FormatVersion XML element")
                .WithInnerException<Exception>("Path Root/Header/FormatVersion not found");
        }

        [Test]
        public void GetFormatVersion_Should_Throw_When_InvalidVersion()
        {
            const string xml = "<Root><Header><FormatVersion>0.42</FormatVersion></Header></Root>";

            Action act = () => GldfFormatVersionReader.GetFormatVersion(xml);

            act.Should()
                .ThrowExactly<GldfException>()
                .WithMessage("Failed to parse FormatVersion element");
        }
    }
}