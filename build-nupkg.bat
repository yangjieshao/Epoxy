@echo off
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\artifacts Epoxy.Wpf\Epoxy.Wpf.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\artifacts Epoxy.Uwp\Epoxy.Uwp.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\artifacts Epoxy.Xamarin.Forms\Epoxy.Xamarin.Forms.csproj
