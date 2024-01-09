using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegramm_task.Core.Core
{
    public enum CommandEnum
    {
        None = 0,
        Add = 1,
        Remove = 2,
        Completed =3,
        [Description("Supervisor")]
        Supervisor = 6,

        [Description("Subdivision")]
        Subdivision = 7,
    }
}
