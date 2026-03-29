namespace ERP.HRM.API.Attributes
{
    /// <summary>
    /// Attribute to mark methods/endpoints that support caching
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheableAttribute : Attribute
    {
        public int DurationSeconds { get; set; }
        public string? CacheKey { get; set; }

        public CacheableAttribute(int durationSeconds = 300, string? cacheKey = null)
        {
            DurationSeconds = durationSeconds;
            CacheKey = cacheKey;
        }
    }

    /// <summary>
    /// Attribute to mark methods that invalidate cache
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InvalidateCacheAttribute : Attribute
    {
        public string[]? CacheKeys { get; set; }

        public InvalidateCacheAttribute(params string[] cacheKeys)
        {
            CacheKeys = cacheKeys;
        }
    }
}
