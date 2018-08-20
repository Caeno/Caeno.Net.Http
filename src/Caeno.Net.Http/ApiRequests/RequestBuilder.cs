using System;
using System.Collections.Generic;
using System.Linq;
using Caeno.Net.Http.Models;
using Caeno.Net.Http.RequestContents;
using System.Globalization;

namespace Caeno.Net.Http.ApiRequests
{
    public class RequestBuilder
    {

        public static RequestBuilder Builder => new RequestBuilder();

		internal RequestContent content;
        RequestMethod method = RequestMethod.Get;
        string path;
        Lazy<Dictionary<string, string>> headers = new Lazy<Dictionary<string, string>>();
        Lazy<Dictionary<string, object>> queryStringParams = new Lazy<Dictionary<string, object>>();

        public RequestBuilder Method(RequestMethod method)
        {
            this.method = method;
            return this;
        }

        public RequestBuilder Path(string path)
        {
            this.path = path;
            return this;
        }

        public RequestBuilder AddQueryParam(string key, object value) {
            if (value is double doubleValue)
                queryStringParams.Value.Add(key, doubleValue.ToString("G", CultureInfo.InvariantCulture));
            else
                queryStringParams.Value.Add(key, value);
            
            return this;
        }

        public RequestBuilder AddQueryParams(params KeyValuePair<string, object>[] items)
        {
            foreach (var item in items)
                queryStringParams.Value.Add(item.Key, item.Value);

            return this;
        }

        public RequestBuilder AddHeader(string key, string value)
        {
            headers.Value.Add(key, value);
            return this;
        }

        public RequestBuilder AddHeaders(params KeyValuePair<string, string>[] items)
        {
            foreach (var item in items)
                headers.Value.Add(item.Key, item.Value);

            return this;
        }

        public RequestBuilder JsonBody(object content)
        {
            this.content = new JsonRequestContent(content);
            return this;
        }

        public IApiRequest Build()
        {
            var request = new ApiRequest
            {
                Method = method,
                Content = content,
                Path = path,
            };

            if (headers.IsValueCreated)
                request.Headers = headers.Value;

            if (queryStringParams.IsValueCreated)
                request.QueryStringParameters = queryStringParams.Value;

            return request;
        }

        class ApiRequest : IApiRequest
        {
            public RequestMethod Method { get; set; }

            public string Path { get; set; }

            public IDictionary<string, object> QueryStringParameters { get; set; }

            public RequestContent Content { get; set; }

            public IDictionary<string, string> Headers { get; set; }
        }

    }

  //  public class JsonBodyBuilder 
  //  {

		//readonly RequestBuilder requestBuilder;
    //    Dictionary<string, object> jsonProperties = new Dictionary<string, object>();

    //    public JsonBodyBuilder(RequestBuilder requestBuilder)
    //    {
    //        this.requestBuilder = requestBuilder;
    //    }



    //    public RequestBuilder Complete() {
    //        requestBuilder.content = new JsonRequestContent()
    //        return requestBuilder;
    //    }
    //}


}
