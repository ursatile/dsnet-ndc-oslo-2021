dotnet clean dotnet\autobarn.sln
dotnet build dotnet\autobarn.sln
wt -M -d . python python\autobarn.py ^
split-pane -H -d . dotnet run --project dotnet\Autobarn.PricingClient; ^
move-focus up; ^
split-pane -V -d . dotnet run --project dotnet\Autobarn.Website; ^
move-focus down; ^
split-pane -H -d . dotnet run --project dotnet\Autobarn.AuditLog; ^
move-focus down; ^
split-pane -H -d . dotnet run --project dotnet\Autobarn.Notifier
