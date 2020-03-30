﻿////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
// Copyright (c) 2020 Kouji Matsui (@kozy_kekyo, @kekyo2)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Epoxy
{
    [DebuggerDisplay("{PrettyPrint}")]
    public abstract class ViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private Dictionary<string, object?>? properties;

        protected ViewModel()
        { }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        private Dictionary<string, object?> Prepare()
        {
            if (!(properties is Dictionary<string, object>))
            {
                properties = new Dictionary<string, object?>();
            }
            return properties;
        }

        public ValueHolder GetValue(
            object? defaultValue = default,
            [CallerMemberName] string? propertyName = null)
        {
            Debug.Assert(propertyName is string);
            Debug.Assert(
                SynchronizationContext.Current != null,
                "Cannot use GetValue() on worker thread context.");

            var properties = this.Prepare();
            if (properties.TryGetValue(propertyName!, out var value))
            {
                return new ValueHolder(value);
            }
            else
            {
                return new ValueHolder(defaultValue);
            }
        }

        public TValue GetValue<TValue>(
            TValue defaultValue = default,
            [CallerMemberName] string? propertyName = null)
        {
            Debug.Assert(propertyName is string);
            Debug.Assert(
                SynchronizationContext.Current != null,
                "Cannot use GetValue<TValue>() on worker thread context.");

            var properties = this.Prepare();
            if (properties.TryGetValue(propertyName!, out var value) && value is TValue v)
            {
                return v;
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetValue<TValue>(
            TValue newValue,
            Action<TValue>? propertyChanged = null,
            [CallerMemberName] string? propertyName = null)
        {
            Debug.Assert(propertyName is string);
            Debug.Assert(
                SynchronizationContext.Current != null,
                "Cannot use SetValue() on worker thread context.");

            var properties = this.Prepare();
            if (properties.TryGetValue(propertyName!, out var oldValue))
            {
                if (!ValueEquals(oldValue, newValue))
                {
                    this.OnPropertyChanging(propertyName);
                    properties[propertyName!] = newValue!;
                    this.OnPropertyChanged(propertyName);
                    propertyChanged?.Invoke(newValue);
                }
            }
            else
            {
                this.OnPropertyChanging(propertyName);
                properties.Add(propertyName!, newValue!);
                this.OnPropertyChanged(propertyName);
                propertyChanged?.Invoke(newValue);
            }
        }

        private static bool ValueEquals<TValue>(
            object? oldValue,
            TValue newValue)
        {
            if ((oldValue == null) && (newValue == null))
            {
                return true;
            }
            else if((oldValue == null) && (newValue != null))
            {
                return false;
            }
            else if((oldValue != null) && (newValue == null))
            {
                return false;
            }
            else
            {
                return oldValue!.Equals(newValue);
            }
        }

        protected void OnPropertyChanging(
            [CallerMemberName] string? propertyName = null)
        {
            Debug.Assert(propertyName is string);
            Debug.Assert(
                SynchronizationContext.Current != null,
                "Cannot use OnPropertyChanging() on worker thread context.");

            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            Debug.Assert(propertyName is string);
            Debug.Assert(
                SynchronizationContext.Current != null,
                "Cannot use OnPropertyChanged() on worker thread context.");

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected Command CreateCommand(
            Func<ValueTask> executeAsync) =>
            new DelegatedAsyncCommand(executeAsync);

        protected Command CreateCommand(
            Func<ValueTask> executeAsync,
            Func<bool> canExecute) =>
            new DelegatedAsyncCommand(executeAsync, canExecute);

        protected Command CreateCommand(
            Action execute) =>
            new DelegatedCommand(execute);

        protected Command CreateCommand(
            Action execute,
            Func<bool> canExecute) =>
            new DelegatedCommand(execute, canExecute);

        protected Command CreateCommand<TParameter>(
            Func<TParameter, ValueTask> executeAsync) =>
            new DelegatedAsyncCommand<TParameter>(executeAsync);

        protected Command CreateCommand<TParameter>(
            Func<TParameter, ValueTask> executeAsync,
            Func<TParameter, bool> canExecute) =>
            new DelegatedAsyncCommand<TParameter>(executeAsync, canExecute);

        protected Command CreateCommand<TParameter>(
            Action<TParameter> execute) =>
            new DelegatedCommand<TParameter>(execute);

        protected Command CreateCommand<TParameter>(
            Action<TParameter> execute,
            Func<TParameter, bool> canExecute) =>
            new DelegatedCommand<TParameter>(execute, canExecute);

        public string PrettyPrint =>
            string.Join(
                ",",
                this.GetType().GetProperties().
                Where(p =>
                    p.CanRead &&
                    (p.GetGetMethod() != null) &&
                    (p.GetIndexParameters().Length == 0) &&
                    (p.PropertyType.IsPrimitive || p.PropertyType == typeof(string))).
                OrderBy(p => p.Name).
                Select(p => $"{p.Name}={p.GetValue(this)}"));

        public override string ToString() =>
            this.PrettyPrint;
    }
}
