using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class CustomFile
    {
        public byte[] FileContents { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}