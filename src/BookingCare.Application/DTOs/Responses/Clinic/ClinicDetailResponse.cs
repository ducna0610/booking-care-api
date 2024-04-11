﻿namespace BookingCare.Application.DTOs.Responses.Clinic
{
    public class ClinicDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DescriptionId { get; set; }
        public string Image { get; set; }
        public int WardId { get; set; }

        public string Description { get; set; }
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
    }
}
