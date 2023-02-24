﻿////////////////////////////////////////////////////////////////////////////
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

using Epoxy;

using Avalonia.Media.Imaging;

namespace EpoxyHello.Avalonia.ViewModels
{
    public sealed class ItemViewModel : ViewModel
    {
        public string? Title
        {
            get => GetValue();
            set => SetValue(value);
        }

        public Bitmap? Image
        {
            get => GetValue();
            set => SetValue(value);
        }

        public int Score
        {
            get => GetValue();
            set => SetValue(value);
        }
    }
}
