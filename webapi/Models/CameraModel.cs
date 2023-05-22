using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using WebApi.Enums;

namespace WebApi.Models
{
    public class CameraOneInputObject
    {
        public CameraOneSource Source { get; set; }
        public List<CameraOneData> Data { get; set; }
    }
    public class CameraOneSource
    {
        public string ReportTime { get; set; }
        public string GroupID { get; set; }
        public string DeviceID { get; set; }
        public string ModelName { get; set; }
        public string MacAddress { get; set; }
        public string IPAddress { get; set; }
        public string TimeZone { get; set; }
        public string DST { get; set; }
    }
    public class CameraOneData
    {
        public string RuleType { get; set; }
        public List<CameraOneCountingInfo> CountingInfo { get; set; }
        public List<CameraOneZoneInfo> ZoneInfo { get; set; }
    }
    public class CameraOneCountingInfo
    {
        public string RuleName { get; set; }
        public int In { get; set; }
        public int Out { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class CameraOneZoneInfo
    {
    }
    /*[XmlRoot("EventNotificationAlert")]
    public class CameraTwoInputObject
    {
        [XmlElement("ipAddress")]
        public string IpAddress { get; set; }
        [XmlElement("protocolType")]
        public string ProtocolType { get; set; }
        [XmlElement("macAddress")]
        public string MacAddress { get; set; }
        [XmlElement("channelID")]
        public string ChannelId { get; set; }
        [XmlElement("dateTime")]
        public string DateTime { get; set; }
        [XmlElement("activePostCount")]
        public string ActivePostCount { get; set; }
        [XmlElement("eventType")]
        public string EventType { get; set; }
        [XmlElement("eventState")]
        public string EventState { get; set; }
        [XmlElement("eventDescription")]
        public string EventDescription { get; set; }
        [XmlElement("channelName")]
        public string ChannelName { get; set; }
        [XmlElement("peopleCounting")]
        public CameraTwoPeopleCounting PeopleCounting { get; set; }
        [XmlElement("childCounting")]
        public CameraTwoChildCounting ChildCounting { get; set; }
    }
    [XmlRoot("CameraTwoPeopleCounting")]
    public class CameraTwoPeopleCounting
    {
        [XmlElement("statisticalMethods")]
        public string StatisticalMethods { get; set; }
        [XmlElement("TimeRange")]
        public CameraTwoTimeRange TimeRange { get; set; }
        [XmlElement("enter")]
        public int Enter { get; set; }
        [XmlElement("exit")]
        public int Exit { get; set; }
        [XmlElement("regionsID")]
        public string RegionsId { get; set; }
    }
    [XmlRoot("CameraTwoTimeRange")]
    public class CameraTwoTimeRange
    {
        [XmlElement("startTime")]
        public string StartTime { get; set; }
        [XmlElement("endTime")]
        public string EndTime { get; set; }
    }
    [XmlRoot("CameraTwoChildCounting")]
    public class CameraTwoChildCounting
    {
        [XmlElement("enter")]
        public int Enter { get; set; }
        [XmlElement("exit")]
        public int Exit { get; set; }
    }*/
    public class CameraTwoInputObject
    {
        public EventNotificationAlert EventNotificationAlert { get; set; }
    }
    public class EventNotificationAlert
    {
        public string IpAddress { get; set; }
        public string ProtocolType { get; set; }
        public string MacAddress { get; set; }
        public string ChannelId { get; set; }
        public string DateTime { get; set; }
        public string ActivePostCount { get; set; }
        public string EventType { get; set; }
        public string EventState { get; set; }
        public string EventDescription { get; set; }
        public string ChannelName { get; set; }
        public CameraTwoPeopleCounting PeopleCounting { get; set; }
        public CameraTwoChildCounting ChildCounting { get; set; }
    }
    public class CameraTwoPeopleCounting
    {
        public string StatisticalMethods { get; set; }
        public CameraTwoTimeRange TimeRange { get; set; }
        public CameraTwoRealTime RealTime { get; set; }
        public int Enter { get; set; }
        public int Exit { get; set; }
        public string RegionsId { get; set; }
    }
    public class CameraTwoTimeRange
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class CameraTwoRealTime
    {
        public string Time { get; set; }
    }
    public class CameraTwoChildCounting
    {
        public int Enter { get; set; }
        public int Exit { get; set; }
    }
    public class CameraQueryInput
    {
        public List<string> CameraIds { get; set; }
        public int ReportType { get; set; }
        public string SearchDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public static string Validate(CameraQueryInput model)
        {
            if (model.ReportType == (int)QueryReportType.ByHours)
            {
                if (string.IsNullOrEmpty(model.SearchDate))
                {
                    return "請指定日期";
                }
            }
            return string.Empty;
        }
    }
    public class CameraQueryOutput
    {
        public int ReportType { get; set; }
        public string QueryTime { get; set; }
        public List<Camera> Cameras { get; set; }
        
    }
    public class Camera
    { 
        public string CameraId { get; set; }
        public List<CameraQueryDetail> CameraDetail { get; set; } 
    }
    public class CameraQueryDetail
    {
        public int PeopleIn { get; set; }
        public int PeopleOut { get; set; }
        public int SearchHour { get; set; }
        public string SearchDate { get; set; }
    }
    public class LoginInput
    { 
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class JwtAuthObject
    { 
        public string Id { get; set; }
    }
}