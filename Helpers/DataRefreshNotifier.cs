using System;

namespace SchoolSystem.Helpers
{
    public static class DataRefreshNotifier
    {
        public static event EventHandler<DataRefreshEventArgs> DataChanged;

        public static void Notify(string scope, object source = null)
        {
            // أي تغيير ناجح في بيانات النظام يجب أن ينعكس على الواجهات المفتوحة.
            // الاحتفاظ بالـscope كتشخيص فقط، مع إرسال حدث عالمي واحد لتجنب اختلاف
            // قوائم الاعتماد بين الشاشات.
            string normalizedScope = string.IsNullOrWhiteSpace(scope)
                ? "All"
                : scope.Trim();

            DataChanged?.Invoke(
                source,
                new DataRefreshEventArgs("All", normalizedScope));
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
        public DataRefreshEventArgs(string scope, string originScope = null)
        {
            Scope = string.IsNullOrWhiteSpace(scope) ? "All" : scope.Trim();
            OriginScope = string.IsNullOrWhiteSpace(originScope) ? Scope : originScope.Trim();
        }

        public string Scope { get; private set; }
        public string OriginScope { get; private set; }
    }
}
