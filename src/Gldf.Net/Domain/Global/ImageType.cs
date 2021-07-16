﻿using System.Xml.Serialization;

namespace Gldf.Net.Domain.Global
{
    public enum ImageType
    {
        [XmlEnum("Product Picture")]
        ProductPicture,

        [XmlEnum("Technical Sketch")]
        TechnicalSketch,

        [XmlEnum("Application Picture")]
        ApplicationPicture,

        [XmlEnum("Other")]
        Other
    }
}