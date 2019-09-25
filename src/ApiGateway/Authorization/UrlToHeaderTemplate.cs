namespace ApiGateway.Authorization
{
    /// <summary>
    /// Template for moving data between URL and http request headers.
    /// </summary>
    public class UrlToHeaderTemplate
    {
        /// <summary>
        /// Part of url to get data from.
        /// </summary>
        public string UrlPart { get; set; }
        /// <summary>
        /// Key of header to put data into.
        /// </summary>
        public string HeaderKey { get; set; }
    }
}