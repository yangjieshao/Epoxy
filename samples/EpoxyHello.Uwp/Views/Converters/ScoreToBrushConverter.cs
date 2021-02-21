﻿////////////////////////////////////////////////////////////////////////////
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

using Epoxy;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace EpoxyHello.Uwp.Views.Converters
{
    public sealed class ScoreToBrushConverter : ValueConverter<Brush, int>
    {
        private static readonly Brush yellow = new SolidColorBrush(Color.FromArgb(255, 96, 96, 0));
        private static readonly Brush gray = new SolidColorBrush(Color.FromArgb(255, 96, 96, 96));

        public override bool TryConvert(int from, out Brush result)
        {
            result = from >= 5 ? yellow : gray;
            return true;
        }
    }
}
