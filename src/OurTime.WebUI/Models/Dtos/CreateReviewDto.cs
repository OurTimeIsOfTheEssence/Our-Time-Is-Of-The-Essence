﻿namespace OurTime.WebUI.Models.Dtos;

public class CreateReviewDto
{
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
