using Entities.DbSet;
using System.Text;
using System.Windows.Input;

namespace GeolocationAppWpf.Commands;

public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    public abstract void Execute(object? parameter);

    protected void OnCanExecutedChanged()         
    { 
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
