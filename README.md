# [Tomer.Bar_Shlomo.com](https://github.com/VRDate/Tomer.Bar_Shlomo.com)
This solution targeting .Net Framwork v4.7.1 is [Jet Brains Raider](https://www.jetbrains/rider/) and [Microsoft Visual Studio](https://visualstudio.microsoft.com/) compatible.

##[Tomer.Bar_Shlomo.com.Logging](https://github.com/VRDate/Tomer.Bar_Shlomo.com/blob/master/Tomer.Bar_Shlomo.com.Logging)
A cunstom thread safe logging library per Type By decending level loging to temporary foldter with Type as file name.

Tested by NUnit [Tomer.Bar_Shlomo.com.Logging.Tests\SmartLoggerTests.cs](https://github.com/VRDate/Tomer.Bar_Shlomo.com/blob/master/Tomer.Bar_Shlomo.com.Logging.Tests/SmartLoggerTests.cs) using:
- CurrentThreadTest
using [Tomer.Bar_Shlomo.com.Logging.Tests\SmartLogGenerator.cs](https://github.com/VRDate/Tomer.Bar_Shlomo.com/blob/master/Tomer.Bar_Shlomo.com.Logging.Tests/SmartLogGenerator.cs)

- MultiThreadTest
using 64 [Tomer.Bar_Shlomo.com.Logging.Tests\SmartLogGenerator.cs](https://github.com/VRDate/Tomer.Bar_Shlomo.com/blob/master/Tomer.Bar_Shlomo.com.Logging.Tests/SmartLogGenerator.cs) threads

- Sample output [Tomer.Bar_Shlomo.com.Logging.Tests\Output\Tomer.Bar_Shlomo.com.Logging.Tests.SmartLogGenerator.log](https://raw.githubusercontent.com/VRDate/Tomer.Bar_Shlomo.com/master/Tomer.Bar_Shlomo.com.Logging.Tests/Output/Tomer.Bar_Shlomo.com.Logging.Tests.SmartLogGenerator.log)

- Microsoft Windows 10
    - Logging to C:\Users\[UserName]\AppData\Local\Temp\Tomer.Bar_Shlomo.com.Logging.Tests.SmartLogGenerator.log
    - Tested with [Microsoft Visual Studio](https://visualstudio.microsoft.com/)
   & [Jet Brains Resharper Extention](https://www.jetbrains/resharper/features/)
   as well as with [Jet Brains Raider](https://www.jetbrains/rider/).
- Linux
    - Logging to /tmp/Tomer.Bar_Shlomo.com.Logging.Tests.SmartLogGenerator.log
    - Tested with [Jet Brains Raider](https://www.jetbrains/rider/)
