using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegramm_task.Core.Core;

public abstract class TaskEvent : ITaskEvent
{
    public abstract event EventHandler AddTaskEvent;
    public abstract event EventHandler RemoveTaskEvent;

    //protected virtual void RemoveEvent(EventArgs e)
    //{
    //    RemoveTaskEvent?.Invoke(this, e);
    //}

    //protected virtual void AddEvent(EventArgs e)
    //{
    //    AddTaskEvent?.Invoke(this, e);
    //}
}