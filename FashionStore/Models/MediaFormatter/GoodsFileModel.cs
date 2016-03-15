using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FashionStore.Core.WebApiFormatters.Interfaces;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Models.MediaFormatter
{
    public class GoodsFileModel:IUploadFile
    {
        public GoodsFileModel()
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