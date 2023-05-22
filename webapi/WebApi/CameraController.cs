using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;
using System;
using WebApi.Core;
using WebApi.Models;
using WebApi.Entities;
using WebApi.Facades;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.Enums;
using System.Text;
using Jose;
using WebApi.Filters;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/camera/{action}")]
    public class CameraController : BaseController
    {
        private ISystemConfig config = Ioc.GetConfig();


        [HttpPost]
        public async Task<IActionResult> CameraOne()
        {
            string message = string.Empty;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                message = await reader.ReadToEndAsync();
            }
            string errorMsg = string.Empty;
            CameraOneInputObject inputObject = JsonConvert.DeserializeObject<CameraOneInputObject>(message);
            CameraName cameraName = CameraFacade.GetCameraName(inputObject.Source.MacAddress);
            if (cameraName != null)
            {
                foreach (var data in inputObject.Data)
                {
                    if (data.RuleType == "Counting")
                    {
                        foreach (var countingInfo in data.CountingInfo)
                        {
                            DateTime.TryParse(countingInfo.StartTime, out DateTime startTime);
                            DateTime.TryParse(countingInfo.StartTime, out DateTime endTime);
                            CameraRaw cameraRaw = new CameraRaw
                            {
                                Mac = inputObject.Source.MacAddress,
                                SectionSpaceId = cameraName.SectionSpaceId,
                                Groupid = inputObject.Source.GroupID,
                                Deviceid = inputObject.Source.DeviceID,
                                Starttime = startTime,
                                Endtime =  endTime,
                                Peoplein = countingInfo.In,
                                Peopleout = countingInfo.Out,
                                Reporttime = DateTime.TryParse(inputObject.Source.ReportTime, out DateTime reportTime) ? reportTime : null
                            };
                            CameraFacade.CameraRawSet(cameraRaw, EntityState.Added);
                        }
                    }

                }
            }
            else
            {
                errorMsg = "查無攝影機";
                var apiLog = new ApiLog
                {
                    Reporttime = DateTime.Now,
                    Api = "CameraOne",
                    Content = JsonConvert.SerializeObject(inputObject) + errorMsg
                };
                CameraFacade.ApiLogSet(apiLog, EntityState.Added);
            }
            return base.Ok(errorMsg);
        }
        [HttpPost]
        public async Task<IActionResult> CameraTwo()
        {
            string message = string.Empty;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                message = await reader.ReadToEndAsync();
            }
            XmlDocument doc = new XmlDocument(); 
            doc.LoadXml(message); 
            string jsontext = JsonConvert.SerializeXmlNode(doc);
            CameraTwoInputObject inputObject = JsonConvert.DeserializeObject<CameraTwoInputObject>(jsontext);
            ApiResult apiResult = new ApiResult();
            CameraName cameraName = CameraFacade.GetCameraName(inputObject.EventNotificationAlert.MacAddress);
            if (cameraName != null)
            {
                DateTime.TryParse(inputObject.EventNotificationAlert.DateTime, out DateTime dateTime);
                if (inputObject.EventNotificationAlert.PeopleCounting.TimeRange != null)
                {
                    DateTime.TryParse(inputObject.EventNotificationAlert.PeopleCounting.TimeRange.StartTime, out DateTime startTime);
                    DateTime.TryParse(inputObject.EventNotificationAlert.PeopleCounting.TimeRange.EndTime, out DateTime endTime);

                    var cameraRaw = new CameraRaw
                    {
                        Mac = inputObject.EventNotificationAlert.MacAddress,
                        SectionSpaceId = cameraName.SectionSpaceId,
                        Groupid = inputObject.EventNotificationAlert.ChannelId,
                        Deviceid = inputObject.EventNotificationAlert.PeopleCounting.RegionsId,
                        Starttime = startTime,
                        Endtime = endTime,
                        Peoplein = inputObject.EventNotificationAlert.PeopleCounting.Enter,
                        Peopleout = inputObject.EventNotificationAlert.PeopleCounting.Exit,
                        Reporttime = dateTime
                    };

                    CameraFacade.CameraRawSet(cameraRaw, EntityState.Added);
                }
                if (inputObject.EventNotificationAlert.PeopleCounting.RealTime != null)
                {
                    DateTime.TryParse(inputObject.EventNotificationAlert.PeopleCounting.RealTime.Time, out DateTime startTime);

                    var cameraRaw = new CameraRaw
                    {
                        Mac = inputObject.EventNotificationAlert.MacAddress,
                        SectionSpaceId = cameraName.SectionSpaceId,
                        Groupid = inputObject.EventNotificationAlert.ChannelId,
                        Deviceid = inputObject.EventNotificationAlert.PeopleCounting.RegionsId,
                        Starttime = startTime,
                        Endtime = startTime,
                        Peoplein = inputObject.EventNotificationAlert.PeopleCounting.Enter,
                        Peopleout = inputObject.EventNotificationAlert.PeopleCounting.Exit,
                        Reporttime = dateTime
                    };

                    CameraFacade.CameraRawSet(cameraRaw, EntityState.Added);
                }
            }
            else
            {
                apiResult.Code = ApiResultCode.Error;
                apiResult.Message = "查無攝影機";
                var apiLog = new ApiLog
                {
                    Reporttime = DateTime.Now,
                    Api = "CameraTwo",
                    Content = JsonConvert.SerializeObject(inputObject) + JsonConvert.SerializeObject(apiResult)
                };
                CameraFacade.ApiLogSet(apiLog, EntityState.Added);
            }
            return base.Ok();
        }
        [HttpPost]
        public ApiResult LoginJwt(LoginInput inputObject)
        {
            ApiResult apiResult = new ApiResult();
            var secret = config.Authorization.Key;

            if (inputObject.UserName == config.Authorization.UserName && inputObject.Password == config.Authorization.Password)
            {
                var expireAt = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                var payload = new Dictionary<string, string>()
                {
                    { "sub", inputObject.UserName },
                    { "exp", expireAt }
                };

                apiResult.Code = ApiResultCode.Success;
                apiResult.Data = new
                {
                    Result = true,
                    Token = JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256),
                    ExpireAt = expireAt
                };
            }
            else
            {
                apiResult.Code = ApiResultCode.InputError;
                apiResult.Message = "帳號密碼錯誤";
            }
            return apiResult;
        }
        [HttpPost]
        public ApiResult GetCameras()
        {
            ApiResult apiResult = new ApiResult();
            List<CameraName> cameraName = CameraFacade.GetCameraNameList();
            if (cameraName != null)
            {
                apiResult.Code = ApiResultCode.Success;
                apiResult.Data = cameraName;
            }
            else
            {
                apiResult.Code = ApiResultCode.Error;
                apiResult.Message = "查無資料";
            }
            return apiResult;
        }
        [HttpPost]
        [JwtAuthActionFilter]
        public ApiResult CameraQuery(CameraQueryInput inputObject)
        {
            ApiResult apiResult = new ApiResult();
            string errMsg = CameraQueryInput.Validate(inputObject);
            try
            {
                if (!string.IsNullOrEmpty(errMsg))
                {
                    apiResult.Code = ApiResultCode.Error;
                    apiResult.Message = errMsg;
                    var apiLog = new ApiLog
                    {
                        Reporttime = DateTime.Now,
                        Api = "CameraQuery",
                        Content = JsonConvert.SerializeObject(inputObject) + JsonConvert.SerializeObject(apiResult)
                    };
                    CameraFacade.ApiLogSet(apiLog, EntityState.Added);
                    return apiResult;
                }
                if (inputObject.ReportType == (int)QueryReportType.ByDate)
                {
                    DateTime.TryParse(inputObject.StartDate, out DateTime startDate);
                    DateTime.TryParse(inputObject.EndDate, out DateTime endDate);
                    apiResult.Data = CameraFacade.GetCameraRawListByDate(inputObject.CameraIds, startDate, endDate, (int)QueryReportType.ByDate);
                }
                if (inputObject.ReportType == (int)QueryReportType.ByHours)
                {
                    DateTime.TryParse(inputObject.SearchDate, out DateTime searchDate);
                    DateTime startTime = searchDate.Date;
                    DateTime endTime = searchDate.Date.AddDays(1).AddSeconds(-1);
                    apiResult.Data = CameraFacade.GetCameraRawListByDate(inputObject.CameraIds, startTime, endTime, (int)QueryReportType.ByHours);
                }
                apiResult.Code = ApiResultCode.Success;
            }
            catch (Exception ex)
            {
                apiResult.Code = ApiResultCode.Error;
                apiResult.Message = ex.Message;
                var apiLog = new ApiLog
                {
                    Reporttime = DateTime.Now,
                    Api = "CameraQuery",
                    Content = JsonConvert.SerializeObject(inputObject) + JsonConvert.SerializeObject(apiResult)
                };
                CameraFacade.ApiLogSet(apiLog, EntityState.Added);
            }
            return apiResult;
        }
    }
}
