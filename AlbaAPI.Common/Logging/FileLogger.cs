using System;
using System.IO;
using System.Threading;
using AlbaAPI.Common.Configuration;

namespace AlbaAPI.Common.Logging
{
    /// <summary>
    /// 파일 기반 로거 구현: Logs 폴더에 날짜별로 로그 파일 생성
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly string _logDirectory;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// 기본 생성자: AppSettings에서 로그 디렉토리 경로 가져오기
        /// </summary>
        public FileLogger() : this(GetLogDirectoryFromConfig())
        {
        }

        /// <summary>
        /// AppSettings에서 로그 디렉토리 경로 가져오기
        /// </summary>
        private static string GetLogDirectoryFromConfig()
        {
            try
            {
                var logDir = AppSettings.LogDirectory;
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                return Path.IsPathRooted(logDir) ? logDir : Path.Combine(baseDir, logDir);
            }
            catch
            {
                // 설정을 읽을 수 없으면 기본값 사용
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            }
        }

        /// <summary>
        /// 로그 디렉토리 지정 생성자
        /// </summary>
        public FileLogger(string logDirectory)
        {
            _logDirectory = logDirectory ?? throw new ArgumentNullException(nameof(logDirectory));
            
            // 로그 디렉토리 생성
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void Error(string message, Exception exception)
        {
            Log(LogLevel.Error, message, exception);
        }

        public void Log(LogLevel level, string message)
        {
            Log(level, message, null);
        }

        public void Log(LogLevel level, string message, Exception exception)
        {
            try
            {
                var logEntry = FormatLogEntry(level, message, exception);
                var logFilePath = GetLogFilePath(level);

                // 스레드 안전하게 파일에 쓰기
                lock (_lockObject)
                {
                    File.AppendAllText(logFilePath, logEntry);
                }
            }
            catch
            {
                // 로깅 실패 시 무시 (무한 루프 방지)
            }
        }

        /// <summary>
        /// 로그 엔트리 포맷팅
        /// </summary>
        private string FormatLogEntry(LogLevel level, string message, Exception exception)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logEntry = $"[{timestamp}] [{level}] {message}";

            if (exception != null)
            {
                logEntry += Environment.NewLine;
                logEntry += $"Exception: {exception.GetType().Name}" + Environment.NewLine;
                logEntry += $"Message: {exception.Message}" + Environment.NewLine;
                logEntry += $"Stack Trace: {exception.StackTrace}" + Environment.NewLine;
            }

            logEntry += Environment.NewLine;
            return logEntry;
        }

        /// <summary>
        /// 로그 파일 경로 생성 (레벨별 폴더, 날짜별 파일)
        /// </summary>
        private string GetLogFilePath(LogLevel level)
        {
            var levelFolder = Path.Combine(_logDirectory, level.ToString());
            
            if (!Directory.Exists(levelFolder))
            {
                Directory.CreateDirectory(levelFolder);
            }

            var fileName = $"{level}_{DateTime.Now:yyyy-MM-dd}.log";
            return Path.Combine(levelFolder, fileName);
        }
    }
}

