using Caeno.Net.Http.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Caeno.Net.Http.RequestContents
{

	/// <summary>
	/// Represents a Multipart Form Request Content.
	/// </summary>
    public class MultipartFormRequestContent : RequestContent
    {
		
		public byte[] ContentBytes { get; set; }

		public string Name { get; set; }

		public string FileName { get; set; }

		public string ContentType { get; set; }

        public MultipartFormRequestContent(byte[] contentBytes, string name, string fileName, string contentType)
        {
			ContentBytes = contentBytes;
			Name = name;
            FileName = fileName;
			ContentType = contentType;
        }

		internal override HttpContent AsHttpContent()
        {
            var content = new MultipartFormDataContent();
			var bytesContent = new ByteArrayContent(ContentBytes);
			bytesContent.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType);

			content.Add(bytesContent, Name, FileName);
            return content;
        }

    }
}
