namespace SIS.HTTP.Requests
{
    using SIS.HTTP.Requests.Contracts;
    using System;
    using System.Collections.Generic;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers.Contracts;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Common;
    using System.Linq;
    using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public IHttpSession Session { get; set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(Environment.NewLine);

            string[] requestLine = splitRequestContent[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();

            bool requestHasBody = splitRequestContent.Length > 1;
            this.ParseRequestParametres(splitRequestContent[splitRequestContent.Length - 1], requestHasBody);
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader("Cookie"))
            {
                return;
            }

            var cookieRow = this.Headers.GetHeader("Cookie").ToString();

            var cookiesValues = cookieRow.Replace("Cookie: ", "");

            var cookies = cookiesValues.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var currentCookie in cookies)
            {
                var cookieKeyValuePair = currentCookie.Split("=", 2);

                if (cookieKeyValuePair.Length != 2)
                {
                    throw new BadRequestException();
                }

                var cookieName = cookieKeyValuePair[0];
                var cookieValue = cookieKeyValuePair[1];
                this.Cookies.Add(new HttpCookie(cookieName, cookieValue));
            }
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            var parseResult = Enum.TryParse<HttpRequestMethod>(requestLine[0], out var parsedRequestMethod);

            if (!parseResult)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = parsedRequestMethod;
        }

        private bool ValidateRequestLine(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                throw new BadRequestException();
            }
            if (requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneProtocolFragment)
            {
                return true;
            }
            return false;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            if (string.IsNullOrEmpty(requestLine[1]))
            {
                throw new BadRequestException();
            }

            this.Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            var path = this.Url?.Split('?').FirstOrDefault();

            if (string.IsNullOrEmpty(path))
            {
                throw new BadRequestException();
            }

            this.Path = path;
        }

        private void ParseHeaders(string[] requestHeaders)
        {
            if (!requestHeaders.Any())
            {
                throw new BadRequestException();
            }

            foreach (var requestHeader in requestHeaders)
            {
                if (string.IsNullOrEmpty(requestHeader))
                {
                    return;
                }

                var splitRequestHeader = requestHeader.Split(": ", StringSplitOptions.RemoveEmptyEntries);
                var requestHeaderKey = splitRequestHeader[0];
                var requestHeaderValue = splitRequestHeader[1];

                this.Headers.Add(new HttpHeader(requestHeaderKey, requestHeaderValue));
            }
        }

        private void ParseRequestParametres(string bodyParameters, bool requestHasBody)
        {
            if (this.Url.Contains('?'))
            {
                this.ParseQueryParametres(this.Url);
            }
            if (requestHasBody)
            {
                this.ParseFormDataParameters(bodyParameters);
            }
        }

        private void ParseFormDataParameters(string bodyParametres)
        {
            var formDataKeyValuePairs = bodyParametres.Split('&', StringSplitOptions.RemoveEmptyEntries);
            ExtractRequestParametres(formDataKeyValuePairs, this.FormData);
        }

        private void ParseQueryParametres(string url)
        {
            var splitUrl = this.Url?.Split(new[] { '?', '#' });
            if (splitUrl.Length > 1)
            {
                var queryParametres = splitUrl.Skip(1).ToArray()[0];

                if (string.IsNullOrEmpty(queryParametres))
                {
                    throw new BadRequestException();
                }

                var queryKeyValuePairs = queryParametres.Split('&', StringSplitOptions.RemoveEmptyEntries);
                ExtractRequestParametres(queryKeyValuePairs, this.QueryData);
            }
        }

        private void ExtractRequestParametres(string[] parameterKeyValuePairs, Dictionary<string, object> parametresCollection)
        {
            foreach (var parameterKeyValuePair in parameterKeyValuePairs)
            {
                var keyValuePair = parameterKeyValuePair.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (keyValuePair.Length != 2)
                {
                    throw new BadRequestException();
                }

                var parameterKey = keyValuePair[0];
                var parameterValue = keyValuePair[1];

                parametresCollection[parameterKey] = parameterValue;
            }
        }
    }

}
