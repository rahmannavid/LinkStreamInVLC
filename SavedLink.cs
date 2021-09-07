using System;
using System.Collections.Generic;
using System.Text;

namespace LinkStreamInVLC
{
    public class SavedLink
    {
        public SavedLink() { }

        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public DateTime savedAt { get; set; }

    }
}
