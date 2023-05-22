using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
	[Table("ApiLog")]
	public class ApiLog
	{
		public ApiLog()
		{
			Api = string.Empty;
			Content = string.Empty;
		}
		[Key]
		[Column("SN")]
		public long Sn { get; set; }

		[Column("reportTime")]
		public DateTime Reporttime { get; set; }

		[Column("API")]
		public string Api { get; set; }

		[Column("Content")]
		public string Content { get; set; }
	}
}
