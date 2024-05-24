////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
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
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
#endif

#if WINUI
using Microsoft.UI.Xaml;
#endif

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

#if MAUI
using Microsoft.Maui.Controls;
#endif

#if AVALONIA || AVALONIA11
using Avalonia;
#endif

namespace Epoxy.Internal;

[EditorBrowsable(EditorBrowsableState.Never)]
internal abstract class EventTrampoline<TTarget, TEventArgs>
{
    private protected readonly string EventName;

    private protected EventTrampoline(
        string eventName) =>
        this.EventName = eventName;

    internal void Bind(TTarget target) =>
        this.OnBind(target);
    internal void Release(TTarget target) =>
        this.OnRelease(target);

    protected abstract void OnBind(TTarget target);
    protected abstract void OnRelease(TTarget target);
}

internal sealed class DynamicEventTrampoline<TTarget, TEventArgs> :
    EventTrampoline<TTarget, TEventArgs>
{
    private readonly Func<TEventArgs, ValueTask> action;
    private readonly EventInfo ei;

    public DynamicEventTrampoline(
        string eventName,
        Func<TEventArgs, ValueTask> action) :
        base(eventName)
    {
        if (EventMetadata.GetOrAddEventInfo(typeof(TTarget), this.EventName) is not { } ei)
        {
            throw new ArgumentException(
                $"Could not find an event: {typeof(TTarget).FullName}.{this.EventName}");
        }

        this.action = action;
        this.ei = ei;
    }

    private async void OnFireEvent(object sender, TEventArgs e)
    {
        if (this.action is { } action)
        {
            try
            {
                await action(e);
            }
            catch (Exception ex)
            {
                // HACK: Because the exception will ignore by 'async void' bottom stack,
                //   (And will reraise delaying UnobservedException on finalizer thread.)
                //   This captures logical task context and reraise on UI thread pumps immediately.
                var edi = ExceptionDispatchInfo.Capture(ex);
                InternalUIThread.ContinueOnUIThread(_ => edi.Throw());
            }
        }
    }

    protected override void OnBind(TTarget target) =>
        EventMetadata.AddEvent(this.ei, target!, this.OnFireEvent);

    protected override void OnRelease(TTarget target) =>
        EventMetadata.RemoveEvent(this.ei, target!, this.OnFireEvent);

    public override string ToString() =>
        $"DynamicEventTrampoline: {typeof(TTarget).FullName}.{this.EventName}";
}

internal sealed class StaticEventTrampoline<TTarget, TEventArgs> :
    EventTrampoline<TTarget, TEventArgs>
{
    private readonly IntPtr methodPtr;
    private readonly Action<TTarget, object, IntPtr> adder;
    private readonly Action<TTarget, object, IntPtr> remover;
    private readonly Func<TEventArgs, ValueTask> action;

    public StaticEventTrampoline(
        string eventName,
        Func<TEventArgs, ValueTask> action,
        Action<TTarget, object, IntPtr> adder,
        Action<TTarget, object, IntPtr> remover) :
        base(eventName)
    {
        this.action = action;
        this.adder = adder;
        this.remover = remover;

        Action<object, TEventArgs> handler = this.OnFireEvent;
        Debug.Assert(handler.Target == this);

        this.methodPtr = handler.Method.MethodHandle.GetFunctionPointer();
    }

    private async void OnFireEvent(object sender, TEventArgs e)
    {
        if (this.action is { } action)
        {
            try
            {
                await action(e);
            }
            catch (Exception ex)
            {
                // HACK: Because the exception will ignore by 'async void' bottom stack,
                //   (And will reraise delaying UnobservedException on finalizer thread.)
                //   This captures logical task context and reraise on UI thread pumps immediately.
                var edi = ExceptionDispatchInfo.Capture(ex);
                InternalUIThread.ContinueOnUIThread(_ => edi.Throw());
            }
        }
    }

    protected override void OnBind(TTarget target) =>
        this.adder(target, this, this.methodPtr);

    protected override void OnRelease(TTarget target) =>
        this.remover(target, this, this.methodPtr);

    public override string ToString() =>
        $"StaticEventTrampoline: {typeof(TTarget).FullName}.{this.EventName}";
}
