﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RetriX.Shared.Resources {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RetriX.Shared.Resources.Strings", typeof(Strings).GetTypeInfo().Assembly);
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
        ///   Looks up a localized string similar to The file supplied is not the expected one. Try a different file..
        /// </summary>
        internal static string FileHashMismatchMessage {
            get {
                return ResourceManager.GetString("FileHashMismatchMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong file.
        /// </summary>
        internal static string FileHashMismatchTitle {
            get {
                return ResourceManager.GetString("FileHashMismatchTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while loading the game.
        ///The game file may be corrupt or in a wrong format: try another one..
        /// </summary>
        internal static string GameLoadingFailAlertMessage {
            get {
                return ResourceManager.GetString("GameLoadingFailAlertMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to load game.
        /// </summary>
        internal static string GameLoadingFailAlertTitle {
            get {
                return ResourceManager.GetString("GameLoadingFailAlertTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while running the game.
        ///Restart RetriX and try again..
        /// </summary>
        internal static string GameRunningFailAlertMessage {
            get {
                return ResourceManager.GetString("GameRunningFailAlertMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Game runtime error.
        /// </summary>
        internal static string GameRunningFailAlertTitle {
            get {
                return ResourceManager.GetString("GameRunningFailAlertTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The folder you selected does not contain the game file you are trying to open. Try again..
        /// </summary>
        internal static string SelectFolderInvalidAlertMessage {
            get {
                return ResourceManager.GetString("SelectFolderInvalidAlertMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid folder.
        /// </summary>
        internal static string SelectFolderInvalidAlertTitle {
            get {
                return ResourceManager.GetString("SelectFolderInvalidAlertTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This game is made up of multiple files. Please select the folder that contains them to load it..
        /// </summary>
        internal static string SelectFolderRequestAlertMessage {
            get {
                return ResourceManager.GetString("SelectFolderRequestAlertMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select the game folder.
        /// </summary>
        internal static string SelectFolderRequestAlertTitle {
            get {
                return ResourceManager.GetString("SelectFolderRequestAlertTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Game state saved to slot {0}.
        /// </summary>
        internal static string StateSavedToSlotMessageBody {
            get {
                return ResourceManager.GetString("StateSavedToSlotMessageBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to State saved.
        /// </summary>
        internal static string StateSavedToSlotMessageTitle {
            get {
                return ResourceManager.GetString("StateSavedToSlotMessageTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some files required to run this game are missing. Install them using the settings page and try again..
        /// </summary>
        internal static string SystemUnmetDependenciesAlertMessage {
            get {
                return ResourceManager.GetString("SystemUnmetDependenciesAlertMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required files missing.
        /// </summary>
        internal static string SystemUnmetDependenciesAlertTitle {
            get {
                return ResourceManager.GetString("SystemUnmetDependenciesAlertTitle", resourceCulture);
            }
        }
    }
}