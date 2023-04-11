﻿using Gldf.Net.Domain.Typed;
using Gldf.Net.Domain.Typed.Definition;
using Gldf.Net.Domain.Typed.Definition.Types;
using Gldf.Net.Domain.Typed.Global;
using Gldf.Net.Domain.Typed.Head;
using Gldf.Net.Domain.Typed.Head.Types;
using Gldf.Net.Domain.Xml.Definition;
using Gldf.Net.Domain.Xml.Definition.Types;
using System;
using System.Collections.Generic;

namespace Gldf.Net.Tests.TestData.Spectrums;

public static class SpectrumCompleteTyped
{
    public static RootTyped RootTyped => new()
    {
        Header = new HeaderTyped
        {
            Manufacturer = "DIAL",
            CreationTimeCode = new DateTime(2021, 3, 29, 14, 30, 0, DateTimeKind.Utc),
            CreatedWithApplication = "Visual Studio Code",
            FormatVersion = FormatVersionTyped.V100
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
                    FileName = "eulumdat.ldt",
                    Uri = "https://example.org/eulumdat.ldt"
                },
                new()
                {
                    Id = "spectrumFile",
                    ContentType = FileContentType.SpectrumText,
                    Type = FileType.Url,
                    FileName = "spectrum.txt",
                    Uri = "https://example.org/spectrum.txt"
                }
            },
            Photometries = new List<PhotometryTyped>
            {
                new()
                {
                    Id = "photometry",
                    PhotometryFile = new GldfFileTyped
                    {
                        Id = "eulumdat",
                        ContentType = FileContentType.LdcEulumdat,
                        Type = FileType.Url,
                        FileName = "eulumdat.ldt",
                        Uri = "https://example.org/eulumdat.ldt"
                    }
                }
            },
            Spectrums = new List<SpectrumTyped>
            {
                new()
                {
                    Id = "spectrum-1",
                    SpectrumFile = new GldfFileTyped
                    {
                        Id = "spectrumFile",
                        ContentType = FileContentType.SpectrumText,
                        FileName = "spectrum.txt",
                        Type = FileType.Url,
                        Uri = "https://example.org/spectrum.txt"
                    }
                },
                new()
                {
                    Id = "spectrum-2",
                    Intensities = new[]
                    {
                        new SpectrumIntensityTyped
                        {
                            Wavelength = 380,
                            Intensity = 0.1
                        },
                        new SpectrumIntensityTyped
                        {
                            Wavelength = 385,
                            Intensity = 0.2
                        },
                        new SpectrumIntensityTyped
                        {
                            Wavelength = 390,
                            Intensity = 0.3
                        }
                    }
                }
            },
            Emitter = new List<EmitterTyped>
            {
                new()
                {
                    Id = "emitter",
                    ChangeableEmitterOptions = new[]
                    {
                        new ChangeableLightEmitterTyped
                        {
                            Photometry = new PhotometryTyped
                            {
                                Id = "photometry",
                                PhotometryFile = new GldfFileTyped
                                {
                                    Id = "eulumdat",
                                    ContentType = FileContentType.LdcEulumdat,
                                    Type = FileType.Url,
                                    FileName = "eulumdat.ldt",
                                    Uri = "https://example.org/eulumdat.ldt"
                                }
                            }
                        }
                    }
                }
            }
        },
        ProductDefinitions = new ProductDefinitionsTyped
        {
            ProductMetaData = new ProductMetaDataTyped
            {
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
                            Text = "Variant 1"
                        }
                    },
                    Geometry = new GeometryTyped
                    {
                        EmitterOnly = new EmitterTyped
                        {
                            Id = "emitter",
                            ChangeableEmitterOptions = new[]
                            {
                                new ChangeableLightEmitterTyped
                                {
                                    Photometry = new PhotometryTyped
                                    {
                                        Id = "photometry",
                                        PhotometryFile = new GldfFileTyped
                                        {
                                            Id = "eulumdat",
                                            ContentType = FileContentType.LdcEulumdat,
                                            Type = FileType.Url,
                                            FileName = "eulumdat.ldt",
                                            Uri = "https://example.org/eulumdat.ldt"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    };
}