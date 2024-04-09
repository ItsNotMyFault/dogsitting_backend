using System;

namespace dogsitting_backend.domain
{
    public class ApplicationRole
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

    }
}