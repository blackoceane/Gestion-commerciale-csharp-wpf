using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace L_Joyce.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        // permet à l’interface graphique (Vue) d’être automatiquement mise à jour quand une propriété du ViewModel change.
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public enum ACTIONMODE { ADD, EDIT, DISPLAY ,ADDEN,DELEN};
        private ACTIONMODE _actionModeActuel = ACTIONMODE.DISPLAY;
        public ACTIONMODE ActionModeActuel
        {
            get => _actionModeActuel;
            set
            {
                if (_actionModeActuel != value)  // evite les events pour rien
                {
                    _actionModeActuel = value;
                    OnPropertyChanged();// notifie que ActionModeActuel a changé


                    // notifie aussi les propriétés dérivées
                    OnPropertyChanged("IsEnabled");
                    OnPropertyChanged("IsReadOnly");
                    OnPropertyChanged("IsEnabledList");
                }
            }
        }
        // actif si on n'est pas en mode "affichage"
        public bool IsEnabled
        {
            get
            {
                return ActionModeActuel != ACTIONMODE.DISPLAY;

            }
        }
        //lecture seule si on est en mode "affichage"
        public bool IsReadOnly
        {
            get
            {
                return ActionModeActuel == ACTIONMODE.DISPLAY;

            }
        }
        // peut être redéfini, mais par défaut actif uniquement en mode "DISPLAY"
        public virtual bool IsEnabledList => ActionModeActuel == ACTIONMODE.DISPLAY;

       // Utilisées pour déterminer si certaines commandes sont permises en fonction de l'état actuel.
        public virtual bool CanEndEdit(object obj)
        {
            return ActionModeActuel != ACTIONMODE.DISPLAY;
        }
        public virtual bool CanBeginEdit(object obj)
        {
            return ActionModeActuel == ACTIONMODE.DISPLAY;
        }

      
    }
}
