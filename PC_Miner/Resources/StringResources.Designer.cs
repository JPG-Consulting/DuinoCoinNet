﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PCMiner.Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StringResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StringResources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PCMiner.Resources.StringResources", typeof(StringResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
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
        ///   Busca una cadena traducida similar a  Accepted .
        /// </summary>
        internal static string accepted {
            get {
                return ResourceManager.GetString("accepted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Do you want to add an identifier (name) to this rig? (y/N): .
        /// </summary>
        internal static string ask_rig_identifier {
            get {
                return ResourceManager.GetString("ask_rig_identifier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Enter desired rig name: .
        /// </summary>
        internal static string ask_rig_name {
            get {
                return ResourceManager.GetString("ask_rig_name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Enter your Duino-Coin username: .
        /// </summary>
        internal static string ask_username {
            get {
                return ResourceManager.GetString("ask_username", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a  Block found .
        /// </summary>
        internal static string block_found {
            get {
                return ResourceManager.GetString("block_found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a  Job received: .
        /// </summary>
        internal static string job_received {
            get {
                return ResourceManager.GetString("job_received", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a  Retrieved mining node: .
        /// </summary>
        internal static string mining_node_retrieved {
            get {
                return ResourceManager.GetString("mining_node_retrieved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a  Node message: .
        /// </summary>
        internal static string node_message {
            get {
                return ResourceManager.GetString("node_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a  Miner is outdated (v{0}) server is on v{1}, please download latest version from https://github.com/revoxhere/duino-coin/releases/.
        /// </summary>
        internal static string outdated_miner {
            get {
                return ResourceManager.GetString("outdated_miner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a  Rejected .
        /// </summary>
        internal static string rejected {
            get {
                return ResourceManager.GetString("rejected", resourceCulture);
            }
        }
    }
}