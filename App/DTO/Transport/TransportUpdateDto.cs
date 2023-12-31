﻿using Simbir.GO.Domain.Models;

namespace Simbir.GO.App.DTO.Transport
{
    public class TransportUpdateDto
    {
        public bool CanBeRented { get; set; } = true;
        public string Model { get; set; }
        public string Color { get; set; }
        public string Identifier { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Description { get; set; }
        public double? MinutePrice { get; set; }
        public double? DayPrice { get; set; }
    }
}
