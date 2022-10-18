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

using System;
using System.Threading.Tasks;

using Epoxy.Advanced;
using Epoxy.Internal;

namespace Epoxy.Supplemental;

public static class GlobalServiceAccessorExtension
{
    public static ValueTask ExecuteAsync<TService>(
        this GlobalServiceAccessor accessor,
        Func<TService, Task> action,
        bool ignoreNotPresent = false) =>
        InternalGlobalService.ExecuteAsync<TService>(service => action(service).AsValueTaskVoid(), ignoreNotPresent);

    public static ValueTask<TResult> ExecuteAsync<TService, TResult>(
        this GlobalServiceAccessor accessor,
        Func<TService, Task<TResult>> action) =>
        InternalGlobalService.ExecuteAsync<TService, TResult>(service =>(action(service).AsValueTask()));
}
