using System;

namespace dogsitting_backend.Domain
{
    public class Client
    {

        private Guid id;
        private string name;
        private string phone;

        public Client(string name)
        {
            this.name = name;   
        }
    }
}
