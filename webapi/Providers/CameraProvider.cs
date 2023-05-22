using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApi.Entities;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using WebApi.Interface;
using WebApi.DbContexts;

namespace WebApi.Providers
{
    public class CameraProvider : ICameraProvider
    {
        public CameraName GetCameraNameByMac(string mac)
        {
            using (var dbx = new CommonDbContext())
            {
                return dbx.CameraNameSet.FirstOrDefault(t => t.Mac == mac);
            }
        }
        public List<CameraName> GetCameraNameList()
        {
            using (var dbx = new CommonDbContext())
            {
                return dbx.CameraNameSet.ToList();
            }
        }
        public bool CameraRawSet(CameraRaw input, EntityState entityState)
        {
            using (var dbx = new CommonDbContext())
            {
                dbx.Entry(input).State = entityState;
                return dbx.SaveChanges() > 0;
            }
        }
        public List<CameraRaw> GetCameraRawListByDate(List<string> cameraIds, DateTime startDate, DateTime endDate)
        {
            using (var dbx = new CommonDbContext())
            {
                if (cameraIds != null)
                {
                    return dbx.CameraRawSet.Where(t => cameraIds.Contains(t.SectionSpaceId.ToString()) && t.Starttime >= startDate && t.Endtime <= endDate).OrderBy(t => t.Starttime).ToList();
                }
                else 
                {
                    return dbx.CameraRawSet.Where(t => t.Starttime >= startDate && t.Endtime <= endDate).OrderBy(t => t.Starttime).ToList();
                }
            }
        }
        public bool ApiLogSet(ApiLog input, EntityState entityState)
        {
            using (var dbx = new CommonDbContext())
            {
                dbx.Entry(input).State = entityState;
                return dbx.SaveChanges() > 0;
            }
        }
    }
}
