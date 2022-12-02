using Gldf.Net.Domain.Typed.Definition;
using Gldf.Net.Domain.Xml.Definition;
using Gldf.Net.Parser.Extensions;
using Gldf.Net.Parser.State;
using System.Linq;

namespace Gldf.Net.Parser
{
    internal class SensorTransform
    {
        public static SensorTransform Instance { get; } = new();

        public ParserDto<SensorTyped> Map(ParserDto<GldfFileTyped> filesDto)
        {
            var parserDto = new ParserDto<SensorTyped>(filesDto);
            if (filesDto.Container.Product.GeneralDefinitions.Sensors?.Any() != true) return parserDto;
            foreach (var sensor in filesDto.Container.Product.GeneralDefinitions.Sensors)
                parserDto.Items.Add(Map(sensor, filesDto));
            return parserDto;
        }

        private static SensorTyped Map(Sensor sensor, ParserDto<GldfFileTyped> files)
        {
            return new SensorTyped
            {
                Id = sensor.Id,
                SensorFile = sensor.SensorFileReference != null ? files.GetFileTyped(sensor.SensorFileReference.FileId) : null,
                DetectorCharacteristics = sensor.DetectorCharacteristics,
                DetectionMethods = sensor.DetectionMethods,
                DetectorTypes = sensor.DetectorTypes?.ToArray()
            };
        }
    }
}