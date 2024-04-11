﻿using Microsoft.AspNetCore.Http;

namespace BookingCare.Application.DTOs.Requests.Clinic
{
    public class UpdateClinicRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ViDescription { get; set; }
        public string? EnDescription { get; set; }
        public int? WardId { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
