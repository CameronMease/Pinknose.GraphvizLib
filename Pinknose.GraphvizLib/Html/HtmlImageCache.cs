/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022-2026 Cameron Mease
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
using System.IO;

namespace Pinknose.GraphvizLib.Html
{
    public sealed class ImageDescription
    {
        #region Constructors

        public ImageDescription(string extension, byte[] bytes)
        {
            Extension = extension ?? throw new ArgumentNullException(nameof(extension));
            Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        #endregion Constructors

        #region Properties

        public byte[] Bytes { get; }

        /// <summary>File extension without the dot, such as <c>png</c>.</summary>
        public string Extension { get; }

        #endregion Properties
    }

    public enum Icons
    {
        Warning
    }

    /// <summary>
    /// Images used by HTML labels. Graphviz reads images from files, so a label refers to an image
    /// by id, and a renderer writes the images out and substitutes the paths.
    /// </summary>
    public sealed class HtmlImageCache : IReadOnlyDictionary<Guid, ImageDescription>
    {
        #region Fields

        private readonly Dictionary<Icons, Guid> _iconGuids = new();
        private readonly Dictionary<Guid, ImageDescription> _imageFiles = new();

        #endregion Fields

        #region Properties

        public int Count => _imageFiles.Count;

        public IEnumerable<Guid> Keys => _imageFiles.Keys;

        public IEnumerable<ImageDescription> Values => _imageFiles.Values;

        public ImageDescription this[Guid key] => _imageFiles[key];

        #endregion Properties

        #region Methods

        /// <summary>The text an image's SRC holds until a renderer replaces it with a path.</summary>
        public static string PlaceholderFor(Guid guid) => $"<GUID>{guid}</GUID>";

        public Guid Add(ImageDescription imageDescription)
        {
            if (imageDescription is null)
            {
                throw new ArgumentNullException(nameof(imageDescription));
            }

            var guid = Guid.NewGuid();
            _imageFiles.Add(guid, imageDescription);
            return guid;
        }

        public bool ContainsKey(Guid key) => _imageFiles.ContainsKey(key);

        /// <summary>
        /// The id of a built-in icon, adding it on first use. Added lazily so a cache that never
        /// shows an icon carries none.
        /// </summary>
        public Guid GetIconGuid(Icons icon)
        {
            if (!_iconGuids.TryGetValue(icon, out var guid))
            {
                guid = Add(new ImageDescription("png", LoadIcon(icon)));
                _iconGuids.Add(icon, guid);
            }

            return guid;
        }

        public IEnumerator<KeyValuePair<Guid, ImageDescription>> GetEnumerator() => _imageFiles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _imageFiles.GetEnumerator();

        public bool TryGetValue(Guid key, out ImageDescription value) => _imageFiles.TryGetValue(key, out value!);

        private static byte[] LoadIcon(Icons icon)
        {
            var resourceName = icon switch
            {
                Icons.Warning => "Pinknose.GraphvizLib.Html.Icons.StatusWarning.png",
                _ => throw new ArgumentOutOfRangeException(nameof(icon)),
            };

            using var stream = typeof(HtmlImageCache).Assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"The embedded resource '{resourceName}' is missing.");
            using var memory = new MemoryStream();
            stream.CopyTo(memory);
            return memory.ToArray();
        }

        #endregion Methods
    }
}
