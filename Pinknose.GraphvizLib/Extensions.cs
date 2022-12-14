using Pinknose.GraphvizLib.Atttributes;
using SkiaSharp;
using Svg;
using System.Text;
using System.Xml;

namespace Pinknose.GraphvizLib
{
    public static class GraphvizExtensions
    {
        #region Methods

        public static byte[] GetBytes(this SKBitmap png)
        {
            return png.Encode(SKEncodedImageFormat.Png, 100000).ToArray();
        }

        public static byte[] GetBytes(this SvgDocument svg)
        {
            using MemoryStream stream = new();
            using XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings() { OmitXmlDeclaration = true });

            svg.Write(writer);
            writer.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            byte[] buffer = stream.ToArray();

            string duhh = Encoding.UTF8.GetString(buffer);

            return buffer;
        }

        public static string GetDisplayValue(this Enum theEnum)
        {
            var enumType = theEnum.GetType();
            var memberInfos = enumType.GetMember(theEnum.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
            var valueAttribute = (DisplayValueAttribute)enumValueMemberInfo.GetCustomAttributes(typeof(DisplayValueAttribute), false).FirstOrDefault();
            return valueAttribute?.DisplayValue ?? theEnum.ToString();
        }

        internal static string ConvertToString(this Stream stream)
        {
            using MemoryStream memoryStream = new();
            stream.CopyTo(memoryStream);

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        #endregion Methods
    }
}