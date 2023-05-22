using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
	[Table("位置空間")]
	public class SectionSpace
	{
		public SectionSpace()
		{
		}

		[Key]
		[Column("位置")]
		public Guid Section { get; set; }

		[Key]
		[Column("空間")]
		public Guid Space { get; set; }

		[Column("位置空間ID")]
		public int SectionSpaceId { get; set; }
	}
}
