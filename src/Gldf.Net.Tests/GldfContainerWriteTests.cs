using FluentAssertions;
using Gldf.Net.Container;
using Gldf.Net.Domain;
using Gldf.Net.Exceptions;
using Gldf.Net.Tests.TestData;
using NUnit.Framework;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Gldf.Net.Tests
{
    [TestFixture]
    public class GldfContainerWriteTests
    {
        private GldfContainerWriter _gldfContainerWriter;
        private string _tempFile1;
        private string _tempFile2;

        [SetUp]
        public void SetUp()
        {
            _gldfContainerWriter = new GldfContainerWriter();
            _tempFile1 = Path.GetTempFileName();
            _tempFile2 = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_tempFile1);
            File.Delete(_tempFile2);
        }

        [Test]
        public void WriteToFile_ShouldCreateContainer_InSubDirectory_When_SubDirectoryNotExists()
        {
            var tempFileName = $"{Guid.NewGuid()}.gldf";
            var tempSubDirectory = Path.Combine(Path.GetTempPath(), $"gldf-{DateTime.Now.Ticks}");
            var tempPath = Path.Combine(tempSubDirectory, tempFileName);
            var gldfContainer = new GldfContainer(new Root());

            _gldfContainerWriter.WriteToFile(tempPath, gldfContainer);
            var containerExists = File.Exists(tempPath);
            Directory.Delete(tempSubDirectory, true);

            containerExists.Should().BeTrue();
        }

        [Test]
        public void WriteToFile_ShouldOverwrite_Container_WhenAlreadyExists()
        {
            var gldfContainer1 = new GldfContainer(new Root { Checksum = "Old" });
            var gldfContainer2 = new GldfContainer(new Root { Checksum = "New" });
            var zipArchiveReader = new ZipArchiveReader();

            _gldfContainerWriter.WriteToFile(_tempFile1, gldfContainer1);
            var checksumBefore = zipArchiveReader.ReadContainer(_tempFile1).Product.Checksum;
            _gldfContainerWriter.WriteToFile(_tempFile1, gldfContainer2);
            var checksumAfter = zipArchiveReader.ReadContainer(_tempFile1).Product.Checksum;

            checksumBefore.Should().Be(gldfContainer1.Product.Checksum);
            checksumAfter.Should().Be(gldfContainer2.Product.Checksum);
        }

        [Test]
        public void WriteToFile_ShouldCreate_SameGldfArchive_AsExpected()
        {
            var rootWithHeaderModel = EmbeddedXmlTestData.GetRootWithHeaderModel();
            var gldfContainer = new GldfContainer(rootWithHeaderModel);
            var expectedBytes = EmbeddedGldfTestData.GetGldfWithFiles();
            var zipArchiveReader = new ZipArchiveReader();

            gldfContainer.Assets.Geometries.Add(new ContainerFile("geometry.l3d", new byte[1]));
            gldfContainer.Assets.Geometries.Add(new ContainerFile("geometry.r3d", new byte[2]));
            gldfContainer.Assets.Images.Add(new ContainerFile("image.jpg", new byte[3]));
            gldfContainer.Assets.Images.Add(new ContainerFile("image.png", new byte[4]));
            gldfContainer.Assets.Photometries.Add(new ContainerFile("lvk.ldt", new byte[5]));
            gldfContainer.Assets.Photometries.Add(new ContainerFile("lvk.ies", new byte[6]));
            gldfContainer.Assets.Documents.Add(new ContainerFile("document.docx", new byte[7]));
            gldfContainer.Assets.Sensors.Add(new ContainerFile("sensor.xml", new byte[8]));
            gldfContainer.Assets.Spectrums.Add(new ContainerFile("spectrum.txt", new byte[9]));
            gldfContainer.Assets.Symbols.Add(new ContainerFile("symbol.svg", new byte[10]));
            gldfContainer.Assets.Other.Add(new ContainerFile("project.c4d", new byte[11]));

            _gldfContainerWriter.WriteToFile(_tempFile1, gldfContainer);
            File.WriteAllBytes(_tempFile2, expectedBytes);

            var loadedWrittenArchive = zipArchiveReader.ReadContainer(_tempFile1);
            var expectedContainer = zipArchiveReader.ReadContainer(_tempFile2);

            loadedWrittenArchive.Should().BeEquivalentTo(expectedContainer);
        }

        [Test]
        public void WriteToFile_Should_OnlyCreate_ProductEntry_WhenNoAssets()
        {
            var gldfContainer = new GldfContainer(new Root());
            _gldfContainerWriter.WriteToFile(_tempFile1, gldfContainer);
            using var zipArchive = ZipFile.Open(_tempFile1, ZipArchiveMode.Read);

            var archiveEntryCount = zipArchive.Entries.Count;
            var firstEntryName = zipArchive.Entries.First().Name;

            archiveEntryCount.Should().Be(1);
            firstEntryName.Should().Be("product.xml");
        }

        [Test]
        public void WriteToFile_ShouldThrow_ArgumentNullException_When_PathIsNull()
        {
            Action act = () => _gldfContainerWriter.WriteToFile(null, new GldfContainer());

            act.Should()
                .ThrowExactly<GldfContainerException>()
                .WithMessage($"Failed to create {nameof(GldfContainer)}*")
                .WithInnerExceptionExactly<ArgumentNullException>()
                .WithMessage("Path cannot be null. (Parameter 'path')");
        }

        [Test]
        public void WriteToFile_ShouldThrow_RootNotFoundException_When_ProductIsNull()
        {
            Action act = () => _gldfContainerWriter.WriteToFile(_tempFile1, new GldfContainer(null));

            act.Should()
                .ThrowExactly<GldfContainerException>()
                .WithMessage($"Failed to create {nameof(GldfContainer)}*")
                .WithInnerExceptionExactly<RootNotFoundException>()
                .WithMessage("Product must not be null");
        }
    }
}