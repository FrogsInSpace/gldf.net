using Gldf.Net.Domain.Typed;
using Gldf.Net.Domain.Typed.Definition;
using Gldf.Net.Domain.Typed.Definition.Types;
using Gldf.Net.Domain.Typed.Global;
using Gldf.Net.Domain.Typed.Head;
using Gldf.Net.Domain.Typed.Head.Types;
using Gldf.Net.Domain.Xml.Definition.Types;
using System;
using System.Collections.Generic;

namespace Gldf.Net.Tests.TestData.Variants;

public static class VariantMandatoryTyped
{
    public static RootTyped RootTyped => new()
    {
        Header = new HeaderTyped
        {
            Manufacturer = "DIAL",
            GldfCreationTimeCode = new DateTime(2021, 3, 29, 14, 30, 0, DateTimeKind.Utc),
            CreatedWithApplication = "Visual Studio Code",
            FormatVersion = new FormatVersionTyped { Major = 1, Minor = 0, PreRelease = 2 },
            UniqueGldfId = "3BE556FF-9061-4592-AEB1-1BC9D507280E"
        },
        GeneralDefinitions = new GeneralDefinitionsTyped
        {
            Files = new List<GldfFileTyped>
            {
                new()
                {
                    Id = "eulumdat",
                    ContentType = FileContentType.LdcEulumdat,
                    Type = FileType.Url,
                    Uri = "https://example.org/eulumdat.ldt"
                }
            },
            Photometries = new List<PhotometryTyped>
            {
                new()
                {
                    Id = "photometry",
                    PhotometryFile = new GldfFileTyped
                    {
                        Id = "eulumdat"
                    }
                }
            },
            FixedLightSources = new List<FixedLightSourceTyped>
            {
                new()
                {
                    Id = "fixedLightSource",
                    Name = new[]
                    {
                        new LocaleTyped
                        {
                            Language = "en",
                            Text = "FixedLightSource"
                        }
                    },
                    RatedInputPower = 50
                }
            },
            SimpleGeometries = new List<SimpleGeometryTyped>
            {
                new()
                {
                    Id = "geometry",
                    CuboidGeometry = new SimpleCuboidGeometryTyped
                    {
                        Width = 1,
                        Length = 2,
                        Height = 3
                    },
                    RectangularEmitter = new SimpleRectangularEmitterTyped
                    {
                        Width = 4,
                        Length = 5
                    }
                }
            },
            Emitter = new List<EmitterTyped>
            {
                new()
                {
                    Id = "emitter",
                    FixedEmitterOptions = new FixedLightEmitterTyped[]
                    {
                        new()
                        {
                            Photometry = new PhotometryTyped
                            {
                                Id = "photometry"
                            },
                            FixedLightSource = new FixedLightSourceTyped
                            {
                                Id = "fixedLightSource"
                            },
                            RatedLuminousFlux = 250
                        }
                    }
                }
            }
        },
        ProductDefinitions = new ProductDefinitionsTyped
        {
            ProductMetaData = new ProductMetaDataTyped
            {
                UniqueProductId = "Product 1",
                ProductNumber = new[]
                {
                    new LocaleTyped
                    {
                        Language = "en",
                        Text = "Product number"
                    }
                },
                Name = new[]
                {
                    new LocaleTyped
                    {
                        Language = "en",
                        Text = "Product name"
                    }
                }
            },
            Variants = new List<VariantTyped>
            {
                new()
                {
                    Id = "variant-1",
                    Name = new[]
                    {
                        new LocaleTyped
                        {
                            Language = "en",
                            Text = "Variant 1"
                        }
                    }
                },
                new()
                {
                    Id = "variant-2",
                    Name = new[]
                    {
                        new LocaleTyped
                        {
                            Language = "en",
                            Text = "Variant 2"
                        }
                    },
                    Geometry = new GeometryTyped
                    {
                        Simple = new SimpleGeometryEmitterTyped
                        {
                            Emitter = new EmitterTyped
                            {
                                Id = "emitter"
                            },
                            Geometry = new SimpleGeometryTyped
                            {
                                Id = "geometry"
                            }
                        }
                    }
                }
            }
        }
    };
}