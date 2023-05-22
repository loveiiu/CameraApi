using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
	[Table("攝影機RAW")]
	public class CameraRaw
	{
		public CameraRaw()
		{
		}
		[Key]
		[Column("SN")]
		public long Sn { get; set; }

		[Column("MAC")]
		public string Mac { get; set; }

		[Column("位置空間ID")]
		public int SectionSpaceId { get; set; }

		[Column("GroupID")]
		public string Groupid { get; set; }

		[Column("DeviceID")]
		public string Deviceid { get; set; }

		[Column("StartTime")]
		public DateTime Starttime { get; set; }

		[Column("EndTime")]
		public DateTime Endtime { get; set; }

		[Column("PeopleIN")]
		public int? Peoplein { get; set; }

		[Column("PeopleOut")]
		public int? Peopleout { get; set; }

		[Column("ReportTime")]
		public DateTime? Reporttime { get; set; }
	}
}
