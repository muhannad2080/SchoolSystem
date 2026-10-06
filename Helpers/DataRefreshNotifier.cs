using System;

namespace SchoolSystem.Helpers
{
    public static class DataRefreshNotifier
    {
        public static event EventHandler<DataRefreshEventArgs> DataChanged;

        public static void Notify(string scope, object source = null)
        {
            if (string.IsNullOrWhiteSpace(scope))
                scope = "All";

            DataChanged?.Invoke(
                source,
                new DataRefreshEventArgs(scope.Trim()));
        }

        public static bool IsRelated(DataRefreshEventArgs eventArgs, string scope)
        {
            if (eventArgs == null || string.IsNullOrWhiteSpace(scope))
                return false;

            return string.Equals(eventArgs.Scope, "All", StringComparison.OrdinalIgnoreCase)
                || string.Equals(eventArgs.Scope, scope, StringComparison.OrdinalIgnoreCase);
        }
    }

    public sealed class DataRefreshEventArgs : EventArgs
    {
        public DataRefreshEventArgs(string scope)
        {
            Scope = scope;
        }

        public string Scope { get; private set; }
    }
}
