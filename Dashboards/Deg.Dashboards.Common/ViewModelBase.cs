using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Deg.Dashboards.Common
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            var property = propertyExpression.Body as MemberExpression;
            NotifyPropertyChanged(property.Member.Name);
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetProperty<TX, TProperty>(ref TX property, TX newValue, Expression<Func<TProperty>> propertyExpression)
        {
            return SetProperty(ref property, newValue, propertyExpression, null);
        }

        protected bool SetProperty<TX, TProperty>(ref TX property, TX newValue, Expression<Func<TProperty>> propertyExpression, Action<TX> onChangedEventHandler)
        {
            if (property is ValueType)
            {
                return SetValueProperty(ref property, newValue, propertyExpression, onChangedEventHandler);
            }

            return SetRefProperty(ref property, newValue, propertyExpression, onChangedEventHandler);
        }

        private bool SetValueProperty<TX, TProperty>(ref TX property, TX newValue, Expression<Func<TProperty>> propertyExpression, Action<TX> onChangedEventHandler)
        {
            if (property.Equals(newValue) == false)
            {
                property = newValue;
                NotifyPropertyChanged(propertyExpression);
                if (onChangedEventHandler != null)
                {
                    onChangedEventHandler(newValue);
                }

                return true;
            }

            return false;
        }

        private bool SetRefProperty<TX, TProperty>(ref TX property, TX newValue, Expression<Func<TProperty>> propertyExpression, Action<TX> onChangedEventHandler)
        {
            if ((property == null && newValue != null) || (!(property == null && newValue == null) && property.Equals(newValue) == false))
            {
                property = newValue;
                NotifyPropertyChanged(propertyExpression);
                if (onChangedEventHandler != null)
                {
                    onChangedEventHandler(newValue);
                }

                return true;
            }

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
