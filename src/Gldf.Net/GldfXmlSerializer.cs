﻿using Gldf.Net.Abstract;
using Gldf.Net.Domain.Xml;
using Gldf.Net.Exceptions;
using Gldf.Net.Extensions;
using Gldf.Net.XmlHelper;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gldf.Net;

/// <summary>
///     Provides functionality to serialize instances of type <see cref="Root" /> into GLDF-XML
///     text and to deserialize GLDF-XML text into instances of type <see cref="Root" />.
///     This type is threadsafe.
/// </summary>
public class GldfXmlSerializer : IGldfXmlSerializer
{
    public Encoding Encoding => _settings.Encoding;
    
    private readonly XmlSerializer _xmlSerializer;
    private readonly XmlSerializerNamespaces _xmlNamespaces;
    private readonly XmlWriterSettings _settings;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GldfXmlSerializer" /> class that can serialize
    ///     instances of type <see cref="Root" /> into GLDF-XML text and to deserialize GLDF-XML
    ///     text into instances of type <see cref="Root" />.
    /// </summary>
    public GldfXmlSerializer() : this(new XmlWriterSettings { Indent = true })
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="GldfXmlSerializer" /> class that can serialize
    ///     instances of type <see cref="Root" /> into GLDF-XML text and to deserialize GLDF-XML
    ///     text into instances of type <see cref="Root" /> This overload also specifies
    ///     <see cref="System.Xml.XmlWriterSettings" /> which allows the control of XML indentation, Encoding
    ///     and more.
    /// </summary>
    public GldfXmlSerializer(XmlWriterSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _xmlSerializer = new XmlSerializer(typeof(Root));
        _xmlNamespaces = GldfNamespaceProvider.GetNamespaces();
    }

    /// <summary>
    ///     Converts the parameter of type <see cref="Root" /> into a GLDF-XML text.
    /// </summary>
    /// <param name="root">The value to convert.</param>
    /// <returns>The GLDF XML representation of the value.</returns>
    /// <exception cref="GldfException">Input is invalid. See also InnerException.</exception>
    public string SerializeToXml(Root root)
    {
        if (root == null) throw new ArgumentNullException(nameof(root));

        try
        {
            var stringBuilder = new StringBuilder();
            using var xmlWriter = XmlWriter.Create(stringBuilder, _settings);
            _xmlSerializer.Serialize(xmlWriter, root, _xmlNamespaces);
            return stringBuilder.ToString();
        }
        catch (Exception e)
        {
            throw new GldfException("Failed to serialize Root to XML", e);
        }
    }

    /// <summary>
    ///     Converts the parameter of type <see cref="Root" /> into a GLDF-XML text and writes
    ///     it to a file.
    /// </summary>
    /// <param name="root">The value to convert.</param>
    /// <param name="filePath">The path of the file to write to</param>
    /// <exception cref="GldfException">
    ///     Input is invalid or the filePath cannot be written to. See also InnerException.
    /// </exception>
    public void SerializeToXmlFile(Root root, string filePath)
    {
        if (root == null) throw new ArgumentNullException(nameof(root));
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        try
        {
            using var streamWriter = new StreamWriter(filePath, false, _settings.Encoding);
            using var xmlWriter = XmlWriter.Create(streamWriter, _settings);
            _xmlSerializer.Serialize(xmlWriter, root, _xmlNamespaces);
        }
        catch (Exception e)
        {
            throw new GldfException("Failed to serialize Root to Disk", e);
        }
    }

    /// <summary>
    ///     Converts the parameter of type <see cref="Root" /> into a GLDF-XML text and writes
    ///     it to a file.
    /// </summary>
    /// <param name="root">The value to convert.</param>
    /// <param name="xmlStream"></param>
    /// <exception cref="GldfException">
    ///     Input is invalid or the filePath cannot be written to. See also InnerException.
    /// </exception>
    public void SerializeToXmlStream(Root root, Stream xmlStream)
    {
        if (root == null) throw new ArgumentNullException(nameof(root));

        try
        {
            using var xmlWriter = XmlWriter.Create(xmlStream, _settings);
            _xmlSerializer.Serialize(xmlWriter, root, _xmlNamespaces);
        }
        catch (Exception e)
        {
            throw new GldfException("Failed to serialize Root to Disk", e);
        }
    }

    /// <summary>
    ///     Parses the text representing a GLDF-XML into an instance of type <see cref="Root" />
    /// </summary>
    /// <param name="xml"></param>
    /// <returns>
    ///     Representation of the GLDF-XML text as instance of the type <see cref="Root" />
    /// </returns>
    /// <exception cref="GldfException">Input is invalid GLDF-XML. See also InnerException.</exception>
    public Root DeserializeFromXml(string xml)
    {
        if (xml == null) throw new ArgumentNullException(nameof(xml));

        try
        {
            using var stringReader = new StringReader(xml);
            var deserializedXml = _xmlSerializer.Deserialize(stringReader);
            return (Root)deserializedXml;
        }
        catch (Exception e)
        {
            throw new GldfException("Failed to read XML", e);
        }
    }

    /// <summary>
    ///     Parses the file containing a GLDF-XML into an instance of type <see cref="Root" />
    /// </summary>
    /// <param name="xmlFilePath">The file containing the GLDF-XML</param>
    /// <returns>
    ///     Representation of the GLDF-XML file as instance of the type <see cref="Root" />
    /// </returns>
    /// <exception cref="GldfException">
    ///     Input is invalid GLDF-XML or the filePath cannot be read from. See also InnerException.
    /// </exception>
    public Root DeserializeFromXmlFile(string xmlFilePath)
    {
        if (xmlFilePath == null) throw new ArgumentNullException(nameof(xmlFilePath));

        try
        {
            using var streamIn = new StreamReader(xmlFilePath, _settings.Encoding);
            var deserializedXml = _xmlSerializer.Deserialize(streamIn);
            return (Root)deserializedXml;
        }
        catch (Exception e)
        {
            throw new GldfException($"Failed to deserialize Root from filepath '{xmlFilePath}'", e);
        }
    }

    /// <summary>
    ///     Parses the file containing a GLDF-XML into an instance of type <see cref="Root" />
    /// </summary>
    /// <param name="xmlStream">The stream containing the GLDF-XML</param>
    /// <returns>
    ///     Representation of the GLDF-XML file as instance of the type <see cref="Root" />
    /// </returns>
    /// <exception cref="GldfException">
    ///     Input is invalid GLDF-XML or the filePath cannot be read from. See also InnerException.
    /// </exception>
    public Root DeserializeFromXmlStream(Stream xmlStream)
    {
        if (xmlStream == null) throw new ArgumentNullException(nameof(xmlStream));

        try
        {
            var deserializedXml = _xmlSerializer.Deserialize(xmlStream);
            return (Root)deserializedXml;
        }
        catch (Exception e)
        {
            throw new GldfException("Failed to deserialize Root from stream. See inner exception", e);
        }
        finally
        {
            if (_settings.CloseOutput) xmlStream.DisposeSafe();
        }
    }
}