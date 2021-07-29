////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) 2019-2021 Kouji Matsui (@kozy_kekyo, @kekyo2)
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

#if WINDOWS_WPF
using System.ComponentModel;
using System.Threading;
using System.Windows;
#endif

#if WINDOWS_UWP || UNO
using Windows.ApplicationModel;
using Windows.UI.Xaml;
#endif

#if WINUI
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
#endif

#if AVALONIA
using Avalonia;
using Avalonia.Controls;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

namespace Epoxy.Internal
{
    internal static class InternalDesigner
    {
#if WINDOWS_WPF
        private static readonly ThreadLocal<bool?> designMode = new ThreadLocal<bool?>();
#endif

        public static bool IsDesignTime
        {
#if WINDOWS_WPF
            get
            {
                switch (designMode.Value)
                {
                    case true:
                        return true;
                    case false:
                        return false;
                    default:
                        var f = DesignerProperties.GetIsInDesignMode(new DependencyObject());
                        designMode.Value = f;
                        return f;
                }
            }
#endif
#if WINDOWS_UWP || WINUI || UNO
            get => DesignMode.DesignModeEnabled;
#endif
#if AVALONIA
            get => Design.IsDesignMode;
#endif
#if NOESIS
            // Noesis doesn't support on design time execution.
            get => false;
#endif
#if XAMARIN_FORMS
            get => DesignMode.IsDesignModeEnabled;
#endif
        }
    }
}
