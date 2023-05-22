using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
	[Table("攝影機名稱")]
	public class CameraName
	{
		public CameraName()
		{
			Name = string.Empty;
			Mac = string.Empty;
			Status = string.Empty;
		}

		[Column("攝影機名稱")]
		public string Name { get; set; }

		[Column("廠牌")]
		public string Brand { get; set; }

		[Column("型號")]
		public string Model { get; set; }

		[Key]
		[Column("MAC")]
		public string Mac { get; set; }

		[Column("狀態")]
		public string Status { get; set; }

		[Column("位置空間ID")]
		public int SectionSpaceId { get; set; }
	}
}
