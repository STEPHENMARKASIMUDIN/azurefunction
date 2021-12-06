using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionTest
{
    public class PexelDTO
    {

        public string id { get; set; }
        public class PexelResponseDTO
        {
            public string url { get; set; }
            public Source src { get; set; }
            public string msg { get; set; }
        }
        public class Source
        {
            public string original { get; set; }
            public string large { get; set; }
            public string portrait { get; set; }
        }
    }
}
