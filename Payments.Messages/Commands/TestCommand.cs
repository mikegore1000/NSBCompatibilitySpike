using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Messages.Commands
{
    public class TestCommand
    {
        public string Message { get; }

        private TestCommand()
        {
        }

        public TestCommand(string message)
        {
            Message = message;
        }
    }
}
