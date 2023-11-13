﻿namespace TheGreatC.Domain.DTOs
{
    public class CommandResponse
    {
        public string Message { get; set; }

        public bool IsSuccessful { get; set; }

        public bool IsNotFound { get; set; } = false;
    }
}