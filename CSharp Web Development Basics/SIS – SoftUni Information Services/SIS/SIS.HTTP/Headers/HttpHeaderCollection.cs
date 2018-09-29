namespace SIS.HTTP.Headers
{
    using SIS.HTTP.Headers.Contracts;
    using System;
    using System.Collections.Generic;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            if (header == null)
            {
                throw new ArgumentException($"{nameof(header)} cannot be null.");
            }

            if (this.ContainsHeader(header.Key))
            {
                throw new ArgumentException("This header already exists.");
            }

            CheckForNullOrEmptyValue(header.Key);

            CheckForNullOrEmptyValue(header.Value);

            this.headers.Add(header.Key, header);
        }

        public bool ContainsHeader(string key)
        {
            CheckForNullOrEmptyValue(key);

            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            CheckForNullOrEmptyValue(key);
           
            if (this.ContainsHeader(key))
            {
                return this.headers[key];
            }
            return null;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.headers.Values);
        }

        private void CheckForNullOrEmptyValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{nameof(value)} cannot be null.");
            }
        }
    }
}
