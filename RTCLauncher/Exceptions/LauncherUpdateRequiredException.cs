namespace RTCV.Launcher.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    [SuppressMessage("Design", "CA1064:Exceptions should be public", Justification = "LauncherUpdateRequiredException will always be caught, and RTCLauncher has no external consumers.")]
    internal class LauncherUpdateRequiredException : Exception
    {
        public LauncherUpdateRequiredException(string message) : base(message)
        {
        }

        public LauncherUpdateRequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LauncherUpdateRequiredException()
        {
        }

        protected LauncherUpdateRequiredException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
