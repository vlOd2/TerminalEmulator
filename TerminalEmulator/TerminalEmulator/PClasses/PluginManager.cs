using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TerminalEmulator.Interfaces;

#pragma warning disable
namespace TerminalEmulator
{
    public class PluginManager
    {
        /// <summary>
        /// The plugins directory
        /// </summary>
        public readonly string pluginsDir;
        /// <summary>
        /// The loaded plugins
        /// </summary>
        public readonly List<IPlugin> loadedPlugins = new List<IPlugin>();

        /// <summary>
        /// A class used to manage plugins
        /// </summary>
        /// <param name="pluginsDir"></param>
        public PluginManager(string pluginsDir = "Plugins")
        {
            this.pluginsDir = pluginsDir;
        }

        /// <summary>
        /// Load the plugin with the specified name and return it (or null)
        /// </summary>
        /// <param name="pluginName">Plugin name</param>
        /// <returns>The loaded plugin or null</returns>
        public IPlugin loadPlugin(string pluginName)
        {
            try
            {
                // Check if the plugin is already loaded
                if (this.getPlugin(pluginName, true) != null || 
                    this.getPlugin(pluginName, false) != null)
                {
                    Program.thisProgram.handleError(
                        "loadPlugin",
                        "The plugin is already loaded.",
                        null,
                        false
                    );

                    return null;
                }

                // Get the plugin's path
                string pluginPath = Path.GetFullPath($@"{this.pluginsDir}\{pluginName}.dll");
                // Load the plugin
                Assembly pluginAssembly = Assembly.LoadFile(pluginPath);
                // Final plugin
                IPlugin finalPlugin;

                // Go thru the plugin's types
                foreach (Type assemblyType in pluginAssembly.GetTypes())
                {
                    // Check if the type is IPlugin
                    if (typeof(IPlugin).IsAssignableFrom(assemblyType) &&
                        assemblyType.IsClass)
                    {
                        // If so load it
                        finalPlugin = (IPlugin)Activator.CreateInstance(assemblyType);
                        this.loadedPlugins.Add(finalPlugin);

                        // And enable it
                        Program.thisProgram.cLogger.Log($"Loaded the plugin \"{finalPlugin.pluginName}\". (Version: {finalPlugin.pluginVersion})");
                        finalPlugin.pAN = pluginName;
                        finalPlugin.theProgram = Program.thisProgram;
                        try
                        {
                            finalPlugin.onEnable();
                        }
                        catch (Exception ex)
                        {
                            Program.thisProgram.handleError(
                                "onExecute",
                                "An unhandled exception has occured.",
                                ex,
                                true
                            );
                        }

                        // And return it
                        return finalPlugin;
                    }
                }

                // Return null
                return null;
            }
            catch (Exception ex)
            {
                Program.thisProgram.handleError(
                    "loadPlugin",
                    "A fatal error has occured.",
                    ex,
                    true
                );

                return null;
            }
        }

        /// <summary>
        /// Get the loaded plugin with the specified name (or null)
        /// </summary>
        /// <param name="pluginName">Plugin name</param>
        /// <param name="isAssemblyName">If the plugin name is the assembly name</param>
        /// <returns>The plugin or null</returns>
        public IPlugin getPlugin(string pluginName, bool isAssemblyName = false)
        {
            try
            {
                // Check if the plugin name is the asssembly name
                if (isAssemblyName)
                {
                    // Go thru each loaded plugin
                    foreach (IPlugin loadedPlugin in this.loadedPlugins.ToList())
                    {
                        // If the name patches, return it
                        if (loadedPlugin.pAN == pluginName)
                        {
                            return loadedPlugin;
                        }
                    }
                }
                else
                {
                    // Go thru each loaded plugin
                    foreach (IPlugin loadedPlugin in this.loadedPlugins.ToList())
                    {
                        // If the name patches, return it
                        if (loadedPlugin.pluginName == pluginName)
                        {
                            return loadedPlugin;
                        }
                    }
                }

                // Return null
                return null;
            }
            catch (Exception ex)
            {
                Program.thisProgram.handleError(
                    "getPlugin",
                    "A fatal error has occured.",
                    ex,
                    true
                );

                return null;
            }
        }

        /// <summary>
        /// Unload the plugin with the specified name
        /// </summary>
        /// <param name="targetedPlugin">Plugin name</param>
        public void unloadPlugin(IPlugin targetedPlugin)
        {
            try
            {
                // Check if the targeted plugin is null
                if (targetedPlugin == null)
                {
                    Program.thisProgram.handleError(
                        "unloadPlugin",
                        "The plugin doesn't exist.",
                        null,
                        false
                    );

                    return;
                }

                // Check if the plugin is not loaded
                if (!this.loadedPlugins.Contains(targetedPlugin))
                {
                    Program.thisProgram.handleError(
                        "unloadPlugin",
                        "The plugin is not loaded.",
                        null,
                        false
                    );

                    return;
                }

                // Unload it
                Program.thisProgram.cLogger.Log($"Unloaded the plugin \"{targetedPlugin.pluginName}\". (Version: {targetedPlugin.pluginVersion})");
                try
                {
                    targetedPlugin.onDisable();
                }
                catch (Exception ex)
                {
                    Program.thisProgram.handleError(
                        "onDisable",
                        "An unhandled exception has occured.",
                        ex,
                        true
                    );
                }

                // Remove the plugin from the loaded plugins list
                this.loadedPlugins.Remove(targetedPlugin);

                // Set it to null
                targetedPlugin = null;
            }
            catch (Exception ex)
            {
                Program.thisProgram.handleError(
                    "unloadPlugin",
                    "A fatal error has occured.",
                    ex,
                    true
                );
            }
        }

        /// <summary>
        /// Load all the plugins in the specified directory
        /// </summary>
        public void loadPlugins()
        {
            try
            {
                string[] pluginsInDir = Directory.GetFiles(this.pluginsDir);
                this.loadedPlugins.Clear();

                if (pluginsInDir.Length < 1)
                {
                    return;
                }

                foreach (string pluginPath in pluginsInDir)
                {
                    this.loadPlugin(Path.GetFileNameWithoutExtension(pluginPath));
                }
            }
            catch (Exception ex)
            {
                Program.thisProgram.handleError(
                    "loadPlugins",
                    "A fatal error has occured.",
                    ex,
                    true
                );
            }
        }

        /// <summary>
        /// Unload all the loaded plugins
        /// </summary>
        public void unloadPlugins()
        {
            try
            {
                foreach (IPlugin loadedPlugin in this.loadedPlugins.ToList())
                {
                    this.unloadPlugin(loadedPlugin);
                }

                this.loadedPlugins.Clear();
            }
            catch (Exception ex)
            {
                Program.thisProgram.handleError(
                    "unloadPlugins",
                    "A fatal error has occured.",
                    ex,
                    true
                );
            }
        }
    }
}
