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
open System.Threading.Tasks

open Epoxy
open Epoxy.Internal

[<DebuggerStepThrough>]
[<AutoOpen>]
module private CommandFactoryExtensionGenerator =
    let inline create0 executeAsync = new DelegatedCommand(executeAsync) :> Command
    let inline create1 executeAsync canExecute = new DelegatedCommand(executeAsync, canExecute) :> Command
    let inline createP0 executeAsync = new DelegatedCommand<'TParameter>(executeAsync) :> Command
    let inline createP1 executeAsync canExecute = new DelegatedCommand<'TParameter>(executeAsync, canExecute) :> Command

[<DebuggerStepThrough>]
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module public CommandFactoryExtension =
    type public CommandFactoryInstance with
        member __.create executeAsync =
            create0 (executeAsync |> asFunc0)
        member __.create (executeAsync, canExecute) =
            create1 (executeAsync |> asFunc0) (canExecute |> asFunc0)

        member __.create (executeAsync: 'TParameter -> ValueTask) =
            createP0 (executeAsync |> asFunc1)
        member __.create (executeAsync: 'TParameter -> ValueTask, canExecute) =
            createP1 (executeAsync |> asFunc1) (canExecute |> asFunc1)

        member __.create (executeAsync: unit -> Task) =
            create0 (executeAsync >> taskVoidAsValueTask |> asFunc0)
        member __.create (executeAsync: unit -> Task, canExecute) =
            create1 (executeAsync >> taskVoidAsValueTask |> asFunc0) (canExecute |> asFunc0)

        member __.create (executeAsync: 'TParameter -> Task) =
            createP0 (executeAsync >> taskVoidAsValueTask |> asFunc1)
        member __.create (executeAsync: 'TParameter -> Task, canExecute) =
            createP1 (executeAsync >> taskVoidAsValueTask |> asFunc1) (canExecute |> asFunc1)