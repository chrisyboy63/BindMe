using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindMe.Commands
{
    public interface ICommand
    {
        IEnumerable<string> Args { get; set; }

        void Execute();
    }
}
