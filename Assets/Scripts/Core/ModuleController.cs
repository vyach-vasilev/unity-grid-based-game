using System;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController<T, U>
{
    private readonly Dictionary<U, Module<T, U>> _modules = new();
    private readonly T _data;
    
    public ModuleController(T data)
    {
        _data = data;
    }
    
    public void Register(List<Module<T, U>> modules)
    {
        foreach (var module in modules) 
        { 
            Register(module);
        }
    }
    
    public void Register(Module<T, U> module)
    {
        if (!_modules.ContainsKey(module.Id))
        {
            _modules.Add(module.Id, module);
            module.Execute(_data);
        }
    }
    
    public void Unregister(List<Module<T, U>> modules)
    {
        modules.Reverse();
        foreach (var module in modules) 
        {
            Unregister(module);
        } 
    }
    
    public void Unregister(Module<T, U> module)
    {
        if(_modules.ContainsKey(module.Id))
        {
            _modules.Remove(module.Id);
        }
    }
    
    public void Unregister(U key)
    {
        if(_modules.TryGetValue(key, out var module))
        {
            _modules.Remove(module.Id);
        }
    }

    public Module<T, U> TryGetModule(U key)
    {
        if (!_modules.TryGetValue(key, out var module))
        {
            throw new Exception($"Module with {key} id is not register.");
        }

        return module;
    }
    
    public void Update(List<Module<T, U>> modules)
    {
        foreach (var module in modules) 
        {
            Update(module);
        } 
    }
    
    public void Update(Module<T, U> module)
    {
        module.Update(_data);
    }
}