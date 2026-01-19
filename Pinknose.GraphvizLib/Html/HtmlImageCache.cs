/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022-2024 Cameron Mease
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Pinknose.GraphvizLib.Html
{
    public class ImageDescription
    {
#if NET6_0_OR_GREATER
        public ImageDescription()
        {
        }
#endif

        public ImageDescription(string extension, byte[] bytes)
        {
            Extension = extension;
            Bytes = bytes;
        }

        public string Extension
        {
            get;
#if NET6_0_OR_GREATER
            init;
#else
            private set;
#endif
        }

        public byte[] Bytes
        {
            get;
#if NET6_0_OR_GREATER
            init;
#else
            private set;
#endif
        }
    }

    public enum Icons
    {
        Warning
    }

    public class HtmlImageCache : IReadOnlyDictionary<Guid, ImageDescription>
    {
        public HtmlImageCache()
        {
            AddIconGuid(Icons.Warning, Resources.StatusWarningIcon);
        }

        private readonly Dictionary<Guid, ImageDescription> ImageFiles = new();

        private readonly Dictionary<Icons, Guid> IconGuids = new();

        public IEnumerable<Guid> Keys => ((IReadOnlyDictionary<Guid, ImageDescription>)ImageFiles).Keys;

        public IEnumerable<ImageDescription> Values => ((IReadOnlyDictionary<Guid, ImageDescription>)ImageFiles).Values;

        public int Count => ((IReadOnlyCollection<KeyValuePair<Guid, ImageDescription>>)ImageFiles).Count;

        public ImageDescription this[Guid key] => ((IReadOnlyDictionary<Guid, ImageDescription>)ImageFiles)[key];

        public Guid Add(ImageDescription imageDescription)
        {
            Guid guid = Guid.NewGuid();
            ImageFiles.Add(guid, imageDescription);
            return guid;
        }

        public Guid GetIconGuid(Icons icon) => IconGuids[icon];

        private void AddIconGuid(Icons icon, Bitmap bitmap)
        {
            var guid = Guid.NewGuid();
            IconGuids.Add(icon, guid);

            using MemoryStream stream = new();
            bitmap.Save(stream, bitmap.RawFormat);
            stream.Position = 0;
            var iconBytes = stream.ToArray();

            string format;

            if (bitmap.RawFormat.Guid == ImageFormat.Png.Guid)
            {
                format = "png";
            }
            else
            {
                throw new NotImplementedException();
            }

            ImageFiles.Add(guid, new(format, iconBytes));
        }

        public bool ContainsKey(Guid key)
        {
            return ((IReadOnlyDictionary<Guid, ImageDescription>)ImageFiles).ContainsKey(key);
        }

        public bool TryGetValue(Guid key, out ImageDescription value)
        {
            return ((IReadOnlyDictionary<Guid, ImageDescription>)ImageFiles).TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<Guid, ImageDescription>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Guid, ImageDescription>>)ImageFiles).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)ImageFiles).GetEnumerator();
        }
    }
}