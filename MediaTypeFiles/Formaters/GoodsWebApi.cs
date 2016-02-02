using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using WebShop.EFModel.Model;

namespace WebShop.Models
{
    internal interface IUploadFiles
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

    public class GoodsWebApi:IUploadFiles
    {
        public GoodsWebApi()
        {
            Files = new List<FileData>();
        }
        public int GoodId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Bad name")]
        public string GoodName { get; set; }
        [Required]
        [Range(0, 100000, ErrorMessage = "The price should be in the range of from 0 to 100000")]
        public decimal PriceUsd { get; set; }
        [Range(0, 100000, ErrorMessage = "The count should be in the range of from 0 to 1000")]
        public int GoodCount { get; set; }
        public IList<FileData> Files { get; set; }
        public Good GetGood()
        {
            return Mapper.Map<Good>(this);
        }

    }
}