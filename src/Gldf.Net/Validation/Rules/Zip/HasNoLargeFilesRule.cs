﻿using Gldf.Net.Abstract;
using Gldf.Net.Container;
using Gldf.Net.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gldf.Net.Validation.Rules.Zip;

internal class HasNoLargeFilesRule : IZipArchiveValidationRule
{
    public const int LimitInBytes = 5 * 1024 * 1024;

    public IEnumerable<ValidationHint> Validate(string filePath)
    {
        try
        {
            var toLargeFiles = ZipArchiveReader.GetLargeFileNames(filePath, LimitInBytes).ToArray();
            return toLargeFiles.Any()
                ? ValidationHint.Warning("Large files found. It is recommended to limit the " +
                                         "maximum file size to 5MB each: " +
                                         $"{FlattenFileNames(toLargeFiles)}", ErrorType.TooLargeFiles)
                : ValidationHint.Empty();
        }
        catch (Exception e)
        {
            return ValidationHint.Warning($"The GLDF container '{filePath}' could not be validated " +
                                          "to have no large Files. " +
                                          $"Error: {e.FlattenMessage()}", ErrorType.TooLargeFiles);
        }
    }

    public IEnumerable<ValidationHint> Validate(Stream stream, bool leaveOpen)
    {
        try
        {
            var toLargeFiles = ZipArchiveReader.GetLargeFileNames(stream, leaveOpen, LimitInBytes).ToArray();
            return toLargeFiles.Any()
                ? ValidationHint.Warning("Large files found. It is recommended to limit the " +
                                         "maximum file size to 5MB each: " +
                                         $"{FlattenFileNames(toLargeFiles)}", ErrorType.TooLargeFiles)
                : ValidationHint.Empty();
        }
        catch (Exception e)
        {
            return ValidationHint.Warning("The GLDF container could not be validated " +
                                          "to have no large Files. " +
                                          $"Error: {e.FlattenMessage()}", ErrorType.TooLargeFiles);
        }
    }

    private static string FlattenFileNames(IEnumerable<string> filesWithoutAssets)
    {
        return filesWithoutAssets.Aggregate(string.Empty, (result, fileName)
            => result == string.Empty ? $"{fileName}" : $"{result}, {fileName}");
    }
}