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

using System;

namespace EpoxyHello.Models
{
    public sealed class RedditPost
    {
        public readonly string Title;
        public readonly Uri Url;
        public readonly int Score;

        public RedditPost(string title, Uri url, int score)
        {
            this.Title = title;
            this.Url = url;
            this.Score = score;
        }
    }
}