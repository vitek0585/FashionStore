using System.Collections.Generic;

namespace FashionStore.Core.WebApiFormatters.Interfaces
{
    internal interface IUploadFile
    {
        IList<FileData> Files { get; set; }
    }

    public struct FileData
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }

        public FileData(string fileName, string mimeType, byte[] data) : this()
        {
            FileName = fileName;
            MimeType = mimeType;
            Data = data;
        }
    }

    
}