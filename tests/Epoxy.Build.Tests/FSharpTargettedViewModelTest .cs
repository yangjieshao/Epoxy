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

#nullable enable

using Epoxy.Infrastructure;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Epoxy
{
    [TestFixture]
    public sealed class FSharpTargettedViewModelTest
    {
        private static readonly string epoxyCorePath =
            typeof(IViewModelImplementer).Assembly.Location;
        private static readonly string targetBasePath =
            Path.GetFullPath(Path.Combine(
                typeof(ViewModelInjectorTest).Assembly.Location,
                "..", "..", "..", "..", "..", "FSharp.Epoxy.Build.TestTargets", "bin",
#if DEBUG
                "Debug"
#else
                "Release"
#endif
                ));

        public static string[] TargetPaths =>
            Directory.GetFiles(targetBasePath, "FSharp.Epoxy.Build.TestTargets.dll", SearchOption.AllDirectories);

        [TestCaseSource("TargetPaths")]
        public void ViewModelTest(string targetPath)
        {
            var tfm = Path.GetFileName(Path.GetDirectoryName(targetPath));

            var injectedPath = Path.Combine(
                Path.GetDirectoryName(this.GetType().Assembly.Location)!,
                $"{Path.GetFileNameWithoutExtension(targetPath)}_{tfm}_{nameof(ViewModelTest)}{Path.GetExtension(targetPath)}");

            var injector = new ViewModelInjector(epoxyCorePath, message => Trace.WriteLine(message));
            var actual = injector.Inject(targetPath, injectedPath);
            Assert.IsTrue(actual);

            var assembly = Assembly.LoadFrom(injectedPath);
            var type = assembly.GetTypes().
                First(t => t.FullName == "Epoxy.TargettedViewModel1");

            var vm = (IViewModelImplementer)Activator.CreateInstance(type)!;

            var count = 0;
            var changing = false;
            vm.PropertyChanging += (s, e) => { Assert.IsFalse(changing); changing = true; count++; };
            vm.PropertyChanged += (s, e) => { Assert.IsTrue(changing); changing = false; count++; };

            dynamic dvm = vm;

            ////////////////////////

            Assert.AreEqual("ABC2", dvm.Prop2);
            Assert.AreEqual(0, count);

            dvm.Prop2 = "AAA2";
            Assert.AreEqual(2, count);

            Assert.AreEqual("AAA2", dvm.Prop2);
            Assert.AreEqual(2, count);
        }
    }
}
