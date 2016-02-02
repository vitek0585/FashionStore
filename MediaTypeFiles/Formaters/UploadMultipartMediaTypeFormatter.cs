using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using WebShop.Models;

namespace FashioStore.Application.Formaters
{
    internal class UploadMultipartMediaTypeFormatter<T> : MediaTypeFormatter where T : IUploadFiles, new()
    {
        public UploadMultipartMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(T);
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        private void InitProperties(string field, string value, T obj)
        {
            var pr = typeof(GoodsWebApi).GetProperty(field,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (pr != null)
            {
                try
                {
                    var tmp = Convert.ChangeType(value, pr.PropertyType);
                    pr.SetValue(obj, tmp);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
        public async override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var item = new T();

            var provider = await content.ReadAsMultipartAsync();

            foreach (var contents in provider.Contents)
            {
                var fieldName = contents.Headers.ContentDisposition.Name.Trim('"');
                if (!string.IsNullOrEmpty(contents.Headers.ContentDisposition.FileName))
                {
                    var data = await contents.ReadAsByteArrayAsync();
                    var fileName = contents.Headers.ContentDisposition.FileName.Trim('"');
                    var fileType = contents.Headers.ContentType.MediaType.Trim('"');
                    item.Files.Add(new FileData(fileName, fileType, data));
                }
                else
                {
                    var value = await contents.ReadAsStringAsync();
                    InitProperties(fieldName, value, item);
                }

            }

            return item;
        }
    }
}