// See https://aka.ms/new-console-template for more information

using DeployInstaller;

string Version = $"3.0.{Utils.GetLastTwoDigitOfYear()}.{Utils.GetDayInYear()}{Utils.GetDay()}";
new RevitSetup().CreateInstaller(Version);
new SandboxSetup().CreateInstaller(Version);
new Civil3DSetup().CreateInstaller(Version);


