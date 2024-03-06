using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneMotorsWebApp.Business.Utilities
{
    public static class UriExtension
    {
        public static Uri ExtendPath(this Uri uri, string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
            {
                return uri;
            }

            var queryString = suffix.Contains('?') ? suffix.Split('?')[1] : "";

            var path = suffix.Contains('?') ? suffix.Split('?')[0] : suffix;

            var uriBuilder = new UriBuilder(uri);

            uriBuilder.Path += path;

            if (!string.IsNullOrEmpty(queryString))
            {
                uriBuilder.Query = queryString;
            }

            return uriBuilder.Uri;
        }
    }
}
