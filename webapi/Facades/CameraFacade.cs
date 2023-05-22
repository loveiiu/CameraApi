using System;
using System.IO;
using WebApi.Entities;
using WebApi.Providers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WebApi.Interface;
using WebApi.Core;
using WebApi.Models;
using WebApi.Enums;

namespace WebApi.Facades
{
    public class CameraFacade
    {
        private static ICameraProvider cp = Ioc.Get<ICameraProvider>();
        public static ISystemConfig config = Ioc.GetConfig();

        public static CameraName GetCameraName(string mac)
        {
            return cp.GetCameraNameByMac(mac);
        }
        public static List<CameraName> GetCameraNameList()
        {
            return cp.GetCameraNameList();
        }
        public static bool CameraRawSet(CameraRaw input, EntityState entityState)
        {
            return cp.CameraRawSet(input, entityState);
        }
        public static CameraQueryOutput GetCameraRawListByDate(List<string> cameraIds, DateTime startTime, DateTime endTime, int queryType)
        {
            CameraQueryOutput cameraQueryOutput = new CameraQueryOutput();
            var result = cp.GetCameraRawListByDate(cameraIds, startTime, endTime);
            if (result != null)
            {
                cameraQueryOutput = CountCameraRaws(result, queryType, startTime, endTime);
            }
            return cameraQueryOutput;
        }
        private static CameraQueryOutput CountCameraRaws(List<CameraRaw> cameraRaws, int queryType, DateTime startTime, DateTime endTime)
        {
            CameraQueryOutput cameraQueryOutput = new CameraQueryOutput
            {
                ReportType = queryType,
                QueryTime = startTime.ToString("yyyy/MM/dd HH:mm:ss") + "~" + endTime.ToString("yyyy/MM/dd HH:mm:ss")
            };
            
            if (queryType == (int)QueryReportType.ByDate)
            {
                var countResult = cameraRaws.GroupBy(t => t.SectionSpaceId).ToDictionary(t=>t.Key,t=>t.ToList());
                List<int> keys = countResult.Keys.ToList();
                DateTime dateTimeOld = startTime.Date;
                DateTime dateTimeNew = new DateTime();
                List<Camera> cameras = new List<Camera>();
                foreach(var key in keys)
                {
                    List<CameraQueryDetail> cameraQueryDetails = new List<CameraQueryDetail>();
                    List<CameraRaw> cameraRawList = countResult[key];
                    int peopleIn = 0;
                    int peopleOut = 0;
                    int rawCount = 1;
                    dateTimeOld = cameraRawList.Min(t => t.Starttime).Date;
                    foreach (var cameraRaw in cameraRawList)
                    {
                        dateTimeNew = cameraRaw.Starttime.Date;
                        if (dateTimeNew == dateTimeOld)
                        {
                            peopleIn += (int)cameraRaw.Peoplein;
                            peopleOut += (int)cameraRaw.Peopleout;
                        }
                        if ((dateTimeNew != dateTimeOld))
                        {
                            cameraQueryDetails.Add(new CameraQueryDetail
                            {
                                PeopleIn = peopleIn,
                                PeopleOut = peopleOut,
                                SearchDate = dateTimeOld.Date.ToString("yyyy-MM-dd")
                            });
                            peopleIn = 0;
                            peopleOut = 0;
                        }
                        if (rawCount == cameraRawList.Count)
                        {
                            cameraQueryDetails.Add(new CameraQueryDetail
                            {
                                PeopleIn = peopleIn,
                                PeopleOut = peopleOut,
                                SearchDate = dateTimeOld.Date.ToString("yyyy-MM-dd")
                            });
                            peopleIn = 0;
                            peopleOut = 0;
                        }
                        dateTimeOld = cameraRaw.Starttime.Date;
                        rawCount++;
                    }
                    cameras.Add(new Camera()
                    {
                        CameraId = key.ToString(),
                        CameraDetail = cameraQueryDetails
                    });
                }
                cameraQueryOutput.Cameras = cameras;
            }
            if (queryType == (int)QueryReportType.ByHours)
            {
                var countResult = cameraRaws.GroupBy(t => t.SectionSpaceId).ToDictionary(t => t.Key, t => t.ToList());
                List<int> keys = countResult.Keys.ToList();
                int dateTimeOld = 0;
                int dateTimeNew = startTime.Hour;
                List<Camera> cameras = new List<Camera>();
                foreach (var key in keys)
                {
                    List<CameraQueryDetail> cameraQueryDetails = new List<CameraQueryDetail>();
                    List<CameraRaw> cameraRawList = countResult[key];
                    int peopleIn = 0;
                    int peopleOut = 0;
                    int i = 0;
                    dateTimeOld = cameraRawList.Min(t => t.Starttime).Hour;
                    foreach (var cameraRaw in cameraRawList)
                    {
                        dateTimeNew = cameraRaw.Starttime.Hour;
                        if (dateTimeNew == dateTimeOld)
                        {
                            peopleIn += (int)cameraRaw.Peoplein;
                            peopleOut += (int)cameraRaw.Peopleout;
                        }
                        if ((dateTimeNew != dateTimeOld))
                        {
                            cameraQueryDetails.Add(new CameraQueryDetail
                            {
                                PeopleIn = peopleIn,
                                PeopleOut = peopleOut,
                                SearchHour = dateTimeOld
                            });
                            peopleIn = 0;
                            peopleOut = 0;
                        }
                        if (i == (cameraRawList.Count-1))
                        {
                            cameraQueryDetails.Add(new CameraQueryDetail
                            {
                                PeopleIn = peopleIn,
                                PeopleOut = peopleOut,
                                SearchHour = dateTimeOld
                            });
                            peopleIn = 0;
                            peopleOut = 0;
                        }
                        dateTimeOld = cameraRaw.Starttime.Hour;
                        i++;
                    }
                    cameras.Add(new Camera()
                    {
                        CameraId = key.ToString(),
                        CameraDetail = cameraQueryDetails
                    });
                }
                cameraQueryOutput.Cameras = cameras;
            }
            return cameraQueryOutput;
        }
        public static bool ApiLogSet(ApiLog input, EntityState entityState)
        {
            return cp.ApiLogSet(input, entityState);
        }
    }
}
