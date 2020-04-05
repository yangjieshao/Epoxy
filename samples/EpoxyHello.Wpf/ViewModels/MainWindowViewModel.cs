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

using Epoxy;
using EpoxyHello.Wpf.Controls;
using EpoxyHello.Wpf.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace EpoxyHello.Wpf.ViewModels
{
    public sealed class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.Indicators = new ObservableCollection<UIElement>();

            // A handler for fetch button
            this.Fetch = CreateCommand(async () =>
            {
                this.IsEnabled = false;

                var waitingBlock = new WaitingBlock();
                this.Indicators.Add(waitingBlock);

                try
                {
                    // Uses Reddit API
                    var reddits = await Reddit.FetchNewPostsAsync("r/aww");

                    this.Items.Clear();

                    foreach (var reddit in reddits)
                    {
                        this.Items.Add(new ItemViewModel
                        {
                            Title = reddit.Title,
                            Score = reddit.Score,
                            Image = await Reddit.FetchImageAsync(reddit.Url)
                        });
                    }
                }
                finally
                {
                    this.Indicators.Remove(waitingBlock);
                    IsEnabled = true;
                }
            });

            this.IsEnabled = true;
        }

        public bool IsEnabled
        {
            get => GetValue();
            private set => SetValue(value);
        }

        public ObservableCollection<ItemViewModel>? Items
        {
            get => GetValue<ObservableCollection<ItemViewModel>?>();
            private set => SetValue(value);
        }

        public Command? Fetch
        {
            get => GetValue();
            private set => SetValue(value);
        }

        public ObservableCollection<UIElement> Indicators
        {
            get => GetValue<ObservableCollection<UIElement>>();
            private set => SetValue(value);
        }
    }
}
