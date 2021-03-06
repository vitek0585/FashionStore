﻿using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FashionStore.Core.WebApiFormatters.Interfaces;

namespace FashionStore.Core.WebApiFormatters
{
    public class FormDataOnlyFileFormatter:MediaTypeFormatter
    {
        public FormDataOnlyFileFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(FileData);
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

       
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, 
            IFormatterLogger formatterLogger)
        {
            var provider = await content.ReadAsMultipartAsync();
            FileData file = new FileData();
            foreach (var contents in provider.Contents)
            {
                if (!string.IsNullOrEmpty(contents.Headers.ContentDisposition.FileName))
                {
                    var data = await contents.ReadAsByteArrayAsync();
                    var fileName = contents.Headers.ContentDisposition.FileName.Trim('\"');
                    var fileType = contents.Headers.ContentType.MediaType;

                    file = new FileData(fileName, fileType, data);
                }
            }

            return file;
        } 
    }
}