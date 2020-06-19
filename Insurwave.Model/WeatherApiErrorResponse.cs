using System;
using System.Collections.Generic;
using System.Text;

namespace Insurwave.Model
{
    public class ApiError
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class ErrorApiResponse
    {
        public ApiError error { get; set; }
    }
}