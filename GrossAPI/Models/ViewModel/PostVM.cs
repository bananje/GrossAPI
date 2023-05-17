﻿using GrossAPI.Models.DTOModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GrossAPI.Models.ViewModel
{
    public class PostVM
    {
        public PostDTO Post { get; set; }
        public List<string> Image { get; set; }
    }
}
