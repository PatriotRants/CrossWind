using System.Diagnostics;
using System.Text;

namespace MinuteMan.Logging;

#nullable disable
public class Logger {
    private static readonly Lazy<Logger> lazyLogger = new(() => new());
    private static readonly Stream writer = new MemoryStream();

    private TextWriter Writer { get; set; }
    private bool IsConfigured => Writer != null;

    public static Logger GetLogger => lazyLogger.IsValueCreated ? lazyLogger.Value : throw new InvalidOperationException("Logger must be configured with output stream prior to reference.");
    public Encoding Encoder { get; set; } = Encoding.UTF8;

    private Logger() { }

    /// <summary>
    /// Configures only once regardless of new stream
    /// </summary>
    public static Logger Configure(Stream outStream) {
        Logger logger = lazyLogger.Value;

        if(!logger.IsConfigured) {
            logger.Writer = new LogWriter(outStream);
        }

        return logger;
    }

    public void Write(string logEntry) {
        Writer.Write(logEntry);
    }

    public void WriteLine(string logEntry) {
        Writer.WriteLine(logEntry);
    }


    private class LogWriter : StreamWriter {

        public LogWriter(Stream stream) : base(stream) {
            AutoFlush = true;
            Console.SetOut(this);
        }

        public override void Write(ReadOnlySpan<char> buffer) {
            base.Write(buffer);
        }

        public override void WriteLine(ReadOnlySpan<char> buffer) {
            base.WriteLine(buffer);
        }
    }
}
