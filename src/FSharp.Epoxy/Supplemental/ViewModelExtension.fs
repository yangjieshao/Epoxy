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

namespace Epoxy.Supplemental

open System
open System.Diagnostics
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System.Threading.Tasks

open Epoxy
open Epoxy.Internal

[<DebuggerStepThrough>]
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module public ViewModelExtension =
    type public ViewModel with
        member viewModel.setValueAsync<'TValue>(newValue, propertyChanged: 'TValue -> ValueTask, [<Optional; CallerMemberName>] propertyName) =
            viewModel.InternalSetValueAsync<'TValue>(newValue, propertyChanged |> asFunc1, propertyName)
        member viewModel.setValueAsync<'TValue> (newValue, propertyChanged: 'TValue -> Task, [<Optional; CallerMemberName>] propertyName) =
            viewModel.InternalSetValueAsync<'TValue>(newValue, propertyChanged >> taskVoidAsValueTask |> asFunc1, propertyName)