﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Utils.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Utils.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;dictionary&gt;
        ///	&lt;breadcrumbs&gt;
        ///		&lt;items lang=&quot;pl&quot;&gt;
        ///			&lt;item key=&quot;Index&quot;&gt;Przegląd&lt;/item&gt;
        ///			&lt;item key=&quot;Vehicles&quot;&gt;Pojazdy&lt;/item&gt;
        ///			&lt;item key=&quot;Refuels&quot;&gt;Tankowania&lt;/item&gt;
        ///			&lt;item key=&quot;Add&quot;&gt;Dodawanie&lt;/item&gt;
        ///			&lt;item key=&quot;Details&quot;&gt;Szczegóły&lt;/item&gt;
        ///			&lt;item key=&quot;Edit&quot;&gt;Edycja&lt;/item&gt;
        ///			&lt;item key=&quot;Delete&quot;&gt;Usuwanie&lt;/item&gt;
        ///			&lt;item key=&quot;vehicleId&quot;&gt;Pojazd&lt;/item&gt;
        ///		&lt;/items&gt;
        ///		&lt;hiddenitems&gt;
        ///			&lt;item&gt;status&lt;/item&gt;
        ///		&lt;/hiddenitems&gt;
        ///	&lt;/breadcrumbs&gt;
        ///&lt;/dictionary&gt;.
        /// </summary>
        internal static string Dictionary {
            get {
                return ResourceManager.GetString("Dictionary", resourceCulture);
            }
        }
    }
}
