﻿/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the Apache License, Version 2.0, please send an email to 
 * vspython@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Apache License, Version 2.0.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/

using System;
using System.IO;
using Microsoft.PythonTools.Designer;
using Microsoft.Windows.Design.Host;

namespace Microsoft.PythonTools.Project {
    class PythonNonCodeFileNode : CommonNonCodeFileNode {
        private DesignerContext _designerContext;

        public PythonNonCodeFileNode(CommonProjectNode root, ProjectElement e)
            : base(root, e) {
        }

        protected internal Microsoft.Windows.Design.Host.DesignerContext DesignerContext {
            get {
                if (_designerContext == null) {
                    _designerContext = new DesignerContext();
                    //Set the EventBindingProvider for this XAML file so the designer will call it
                    //when event handlers need to be generated
                    var dirName = Path.GetDirectoryName(Url);
                    var fileName = Path.GetFileNameWithoutExtension(Url);
                    var filenameWithoutExt = Path.Combine(dirName, fileName);

                    // look for foo.py
                    var child = Parent.FindChild(filenameWithoutExt + PythonConstants.FileExtension);
                    if (child == null) {
                        // then look for foo.pyw
                        child = Parent.FindChild(filenameWithoutExt + PythonConstants.WindowsFileExtension);
                    }

                    if (child != null) {
                        _designerContext.EventBindingProvider = new WpfEventBindingProvider(child as PythonFileNode);
                    }
                }
                return _designerContext;
            }
        }

        protected override object CreateServices(Type serviceType) {
            object service = null;
            if (typeof(DesignerContext) == serviceType) {
                service = this.DesignerContext;
            } else {
                return base.CreateServices(serviceType);
            }
            return service;
        }


    }
}
