using System.Collections.Generic;

namespace GBank.Domain.Documents
{
    public class Customer : Document
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}