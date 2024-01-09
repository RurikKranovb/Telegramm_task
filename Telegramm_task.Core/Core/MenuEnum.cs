using System.ComponentModel;

namespace Telegramm_task.Core.Core;

public enum MenuEnum
{
    [Description("AddTask")]
    AddTask = 1,

    [Description("Completed")]
    Completed = 2,

    [Description("StateTasks")]
    StateTasks = 3,

    [Description("RemoveTask")]
    RemoveTask = 4,

    [Description("ActiveTasks")]
    ActiveTasks = 5,

    //---------------------------

    [Description("Supervisor")]
    Supervisor = 6,

    [Description("Subdivision")]
    Subdivision = 7,

    //----------------------------

    //[Description("Laser")]
    //Laser = 8,

    //[Description("Bending")]
    //Bending = 9,

    //[Description("Welding")]
    //Welding = 10,
}