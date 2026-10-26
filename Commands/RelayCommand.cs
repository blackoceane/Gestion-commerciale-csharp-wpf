using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace L_Joyce.Commands
{
    public class RelayCommand : ICommand
    {
        //Cette variable stocke l'action qui sera exécutée lorsque la commande est invoquée.
        /// <summary>
        /// /C’est une action personnalisée qui fait quelque chose lorsque la commande est exécutée.
        /// </summary>
        private readonly Action<object> _execute;
        //C'est une condition qui détermine si la commande peut être exécutée ou non.
        //C’est une vérification de permission avant de pouvoir exécuter l'action.
        private readonly Predicate<object> _canExecute;
        //permet de spécifier execute et canExecute 
        public RelayCommand(Action<object> execute, Predicate<Object> canExecute)
        {
            _execute = execute ?? throw new NullReferenceException("execute");
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }
        //Cet événement est déclenché chaque fois qu’une condition d’exécution change
        //ce qui peut être utilisé pour activer/désactiver des boutons ou autres contrôles liés à la commande.
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        //Cette méthode exécute l'action associée à la commande.
        public void Execute(object parameter) => _execute?.Invoke(parameter);
        //Cette méthode vérifie si la commande peut être exécutée, c'est-à-dire si le prédicat _canExecute retourne true ou false
        //Si _canExecute est null, la commande est toujours exécutable.
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute.Invoke(parameter);
        }
    }
}
