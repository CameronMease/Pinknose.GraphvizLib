using SkiaSharp;
using Svg;
using System.Diagnostics;
using System.Reflection;

namespace Pinknose.GraphvizLib
{
    public enum GraphvizEngine
    { Dot, Fdp }

    public static class Dot
    {
        #region Fields

        private static readonly Lazy<string> DotBinPath = new(() =>
            {
                var assemblyLocation = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Dot))?.Location) ?? throw new ArgumentNullException();

                string path = Path.Combine(
                    assemblyLocation,
                    "Graphviz",
                    "bin");

                return path;
            });

        #endregion Fields

        #region Methods

        public static async Task<SKBitmap> RenderPngAsync(Graph graph, GraphvizEngine engine)
        {
            using var stream = await RenderAsync(graph, "png", engine).ConfigureAwait(false);

            stream.Seek(0, SeekOrigin.Begin);

            var bitmap = SKBitmap.Decode(stream);

            return bitmap;
        }

        public static async Task<SvgDocument> RenderSvgAsync(Graph graph, GraphvizEngine engine)
        {
            using var stream = await RenderAsync(graph, "svg", engine).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);

            var svg = SvgDocument.Open<SvgDocument>(stream);

            return svg;
        }

        public static async Task RenderToPngFileAsync(Graph graph, GraphvizEngine engine, string filename)
        {
            using var stream = await RenderAsync(graph, "png", engine).ConfigureAwait(false);

            stream.Seek(0, SeekOrigin.Begin);

            using var filestream = File.Create(filename);

            await stream.CopyToAsync(filestream);

            filestream.Close();
        }

        public static async Task RenderToSvgFileAsync(Graph graph, GraphvizEngine engine, string filename)
        {
            using var stream = await RenderAsync(graph, "svg", engine).ConfigureAwait(false);

            stream.Seek(0, SeekOrigin.Begin);

            using var filestream = File.Create(filename);

            await stream.CopyToAsync(filestream);

            filestream.Close();
        }

        private static async Task<Stream> RenderAsync(Graph graph, string type, GraphvizEngine engine)
        {
            var dotExecutableName = engine switch
            {
                GraphvizEngine.Fdp => "fdp.exe",
                GraphvizEngine.Dot => "dot.exe",
                _ => throw new NotImplementedException()
            };

            bool errorRunningProcess = false;

            string dot = graph.RenderDot();

            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    WorkingDirectory = DotBinPath?.Value,
                    FileName = Path.Combine(DotBinPath?.Value, dotExecutableName),
                    Arguments = $@"-T{type}",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            proc.ErrorDataReceived += (sender, e) =>
            {
                errorRunningProcess = true;
                Debug.Write(e.Data);
            };

            proc.OutputDataReceived += (sender, e) =>
            {
            };

            try
            {
                proc.Start();

                proc.StandardInput.WriteLine(dot);
                proc.StandardInput.Flush();
                proc.StandardInput.Close();

                MemoryStream memoryStream = new();

                byte[] buffer = new byte[8192];
                int lastBytesRead = 0;
                int totalBytesRead = 0;

                do
                {
                    lastBytesRead = await proc.StandardOutput.BaseStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    totalBytesRead += lastBytesRead;
                    memoryStream.Write(buffer, 0, lastBytesRead);
                } while (lastBytesRead > 0);

                await proc.WaitForExitAsync();

                if (errorRunningProcess)
                {
                    Debug.WriteLine("Error occurred while running DOT process.");
                }

                return memoryStream;
            }
            catch (Exception)
            {
            }

            throw new NotImplementedException();
        }

        #endregion Methods
    }
}