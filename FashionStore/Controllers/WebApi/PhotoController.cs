﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FashionStore.Application.Bootstrapper.InversionOfControl;
using FashionStore.Core.AppValue;
using FashionStore.Core.WebApiFormatters.Interfaces;
using FashionStore.Domain.Core.Entities.Store;
using FashionStore.Service.Interfaces.Services;
using FashionStore.Service.Interfaces.UoW;
using WebLogger.Abstract.Interface;
using WebLogger.Abstract.Interface.Sql;

namespace FashionStore.Controllers.WebApi
{
    [RoutePrefix("api/Photo")]
    public class PhotoController : ApiController
    {
        private IUnitOfWork _unit;
        private IImageService _image;
        private ILogWriterSql _log;

        public PhotoController(IUnitOfWorkStore unit, IImageService image, ILogWriterSql log)
        {
            _image = image;
            _log = log;
            _unit = unit;
        }
        [HttpPost]
        [Route("AddPhoto")]
        public async Task<HttpResponseMessage> AddPhoto(int id, FileData file)
        {

            try
            {

                var path = HttpContext.Current.Server.MapPath(ValuesApp.ConvertImageNameToAbsolutePath(file.FileName));
                var img = await _image.AddImage(id, file.Data, path);
                return Request.CreateResponse(HttpStatusCode.Created, img.ImagePath);

            }
            catch (ArgumentException e)
            {
                var msg = string.Format("Photo by id - {0} has been failed save, name is incorrect", id);
                _log.LogWriteInfo(msg);
                return Request.CreateResponse(HttpStatusCode.BadRequest, msg);
            }
            catch (Exception e)
            {
                _log.LogWriteError(string.Format("photo by id - {0} has been failed", id), e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        [HttpPost]
        [Route("RemovePhoto")]
        public HttpResponseMessage RemovePhoto([FromBody]int id)
        {
            try
            {
                //_photo.Delete(_photo.FindBy(p => p.PhotoId == id).First());
                _unit.Save();
                return Request.CreateResponse(HttpStatusCode.Accepted,
                    string.Format("The photo by id {0} was deleted successfuly", id));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    string.Format("The photo by id {0} was not deleted", id));
            }
        }
    }
}
