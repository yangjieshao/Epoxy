﻿////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

#nullable enable

using Epoxy;
using Windows.UI.Xaml.Media;

namespace EpoxyHello.ViewModels
{
    public sealed class ItemViewModel : ViewModel
    {
        public string? Title
        {
            get => GetValue();
            set => SetValue(value);
        }

        public ImageSource? Image
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