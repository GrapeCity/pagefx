//
// AssemblyInfo.cs
//
// Author:
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//
// (C) 2003 Ximian, Inc.  http://www.ximian.com
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Reflection;
#if NOT_PFX
using System.Resources;
#endif
//using System.Security;
//using System.Security.Permissions;
//using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

// General Information about the system assembly

[assembly: AssemblyVersion (Consts.FxVersion)]
#if NOT_PFX
[assembly: SatelliteContractVersion (Consts.FxVersion)]
#endif

//#if (ONLY_1_1)
//[assembly: ComCompatibleVersion (1, 0, 3300, 0)]
//[assembly: TypeLibVersion (1, 10)]
//#endif

//[assembly: AssemblyTitle("mscorlib.dll")]
[assembly: AssemblyDescription("Common Language Runtime Library")]
//[assembly: AssemblyConfiguration("Development version")]
//[assembly: AssemblyCompany("MONO development team")]
//[assembly: AssemblyProduct("MONO CLI")]
//[assembly: AssemblyCopyright("(c) 2003 Various Authors")]

[assembly: CLSCompliant(true)]
//[assembly: AssemblyDefaultAlias("mscorlib.dll")]
//[assembly: AssemblyInformationalVersion("0.0.0.1")]
#if NOT_PFX
[assembly: NeutralResourcesLanguage("en-US")]
#endif

//[assembly: AllowPartiallyTrustedCallers]
[assembly: Guid("BED7F4EA-1A96-11D2-8F08-00A0C9A6186D")]

#if ! BOOTSTRAP_WITH_OLDLIB
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: AssemblyDelaySign(true)]
[assembly: AssemblyKeyFile("../ecma.pub")]
#endif
