using System;
using System.Collections.Generic;
using WebApi.Enums;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Interface
{
    public interface ICameraProvider
    {
        #region 攝影機名稱
        CameraName GetCameraNameByMac(string mac);
        List<CameraName> GetCameraNameList();
        #endregion
        #region 攝影機RAW
        bool CameraRawSet(CameraRaw input, EntityState entityState);
        List<CameraRaw> GetCameraRawListByDate(List<string> cameraIds, DateTime startDate, DateTime endDate);
        #endregion
        #region ApiLog
        bool ApiLogSet(ApiLog input, EntityState entityState);
        #endregion
    }
}
