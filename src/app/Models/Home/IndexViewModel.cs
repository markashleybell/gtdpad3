using System.Collections.Generic;
using GTDPad.Domain;

namespace GTDPad.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Page> Pages { get; set; }

        public Page Page { get; set; }
    }
}
