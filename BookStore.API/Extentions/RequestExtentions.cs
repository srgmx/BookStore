using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace BookStore.API.Extentions
{
    public static class RequestExtentions
    {
        public static string GetCreatedUri(this HttpRequest request, Guid resourceId)
        {
            var uri = $"{request.GetEncodedUrl()}/{resourceId}";

            return uri;
        }
    }
}
