﻿using System;

namespace Messages {
	public class VehicleAddedMessage 
	{
		public string Registration { get; set; }
		public string Manufacturer { get; set; }
		public string ModelName { get; set; }
		public string ModelCode { get; set; }
		public string Color { get; set; }
		public int Year { get; set; }
		public DateTime ListedAtUtc { get; set; }
	}
}
